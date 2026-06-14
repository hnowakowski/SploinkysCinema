import aiohttp
import asyncio
import time
import random

BASE_URL = "https://localhost:7117/Reservation/api/reservations"

ENDPOINTS = [
    {"method": "GET", "url": f"{BASE_URL}/getall"},
    # Add your other API endpoints here to randomize them
    # {"method": "POST", "url": f"{BASE_URL}/book", "json": {"movieId": "...", "seat": 1}},
    # {"method": "DELETE", "url": f"{BASE_URL}/cancel/123"},
]

async def client_task(client_id, session, num_requests):
    successes = 0
    fails = 0
    
    for i in range(num_requests):
        endpoint = random.choice(ENDPOINTS)
        method = endpoint["method"]
        url = endpoint["url"]
        payload = endpoint.get("json", None)
        
        try:
            async with session.request(method, url, json=payload, ssl=False) as response:
                if response.status in (200, 201, 204):
                    # print(f"Client {client_id} success on {method} {url}")
                    successes += 1
                else:
                    print(f"Client {client_id} FAILED {i}, response code {response.status} on {method} {url}")
                    fails += 1
        except Exception as e:
            print(f"Client {client_id} FAILED {i}, exception: {e}")
            fails += 1
            
        # Optional: wait a random short duration before next request
        await asyncio.sleep(random.uniform(0.01, 0.1))

    return successes, fails

async def run():
    print("\n==== STARTING TWO CLIENTS RANDOM REQUESTS =====")
    requests_per_client = 50  # adjust as needed
    
    async with aiohttp.ClientSession() as session:
        # Create two client tasks
        task1 = client_task("Client_A", session, requests_per_client)
        task2 = client_task("Client_B", session, requests_per_client)
        
        start_time = time.perf_counter()
        results = await asyncio.gather(task1, task2)
        end_time = time.perf_counter()
        
        total_time = end_time - start_time
        
        print("\n==== RESULTS =====")
        print(f"Time elapsed: {total_time:.4f}s")
        for idx, (successes, fails) in enumerate(results):
            client_name = "Client_A" if idx == 0 else "Client_B"
            print(f"{client_name}:")
            print(f"  Successes:  {successes}/{requests_per_client}")
            print(f"  Fails:      {fails}/{requests_per_client}")

if __name__ == "__main__":
    asyncio.run(run())