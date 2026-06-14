import aiohttp
import asyncio
import time
import random
from datetime import datetime, timezone

BASE_URL = "https://localhost:7117/Reservation/api/reservations"

# hard to verify the number of 409's and 404's here so i'll only count the number of 500's
# could do multithreading but asyncio also makes all of this get evaluated in non-deterministic order

ENDPOINTS = [
    {"method": "GET", "url": f"{BASE_URL}/getall"},
    {"method": "GET", "url": f"{BASE_URL}/getmovieseats", "json": {"movieId": "ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1"}},
    {"method": "POST", "url": f"{BASE_URL}/post",
      "json": {"movieId": "ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1","movieName": "FNAF2","username": "Freddy Fazbear","seat": 7,"row": 6,"lastUpdate": datetime.now(timezone.utc).isoformat()}},
    {"method": "PUT", "url": f"{BASE_URL}/put",
      "json": {"movieId": "ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1","movieName": "FNAF2","username": "Freddy Fazbear","seat": 7,"row": 6,"lastUpdate": datetime.now(timezone.utc).isoformat()},
      "params": "William Afton"},
    {"method": "DELETE", "url": f"{BASE_URL}/delete",
     "json": {"movieId": "ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1","movieName": "FNAF2","username": "Freddy Fazbear","seat": 7,"row": 6,"lastUpdate": datetime.now(timezone.utc).isoformat()}},
    {"method": "DELETE", "url": f"{BASE_URL}/delete",
     "json": {"movieId": "ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1","movieName": "FNAF2","username": "William Afton","seat": 7,"row": 6,"lastUpdate": datetime.now(timezone.utc).isoformat()}},
]

async def req(client_id, session):
    successes = 0
    fails = 0
    
    for i in range(100):
        if i % 20 == 0:
            print(f"Client {client_id} Iteration {i}")
        endpoint = random.choice(ENDPOINTS)
        method = endpoint["method"]
        url = endpoint["url"]
        payload = endpoint.get("json", None)
        params = endpoint.get("params", None)
        try:
            async with session.request(method, url, json=payload, params=params, ssl=False) as response:
                if response.status == 500:
                    print(f"Client {client_id} FAILED {i}, response code {response.status} on {method} {url}")
                    fails += 1
                else:
                    successes += 1
        except Exception as e:
            print(f"Client {client_id} FAILED {i}, exception: {e}")
            fails += 1
    return successes, fails

async def run():
    print("\n==== TWO CLIENTS SPAMMING 100 RANDOM REQUESTS =====")
    
    async with aiohttp.ClientSession() as session:
        task1 = req("Client_A", session)
        task2 = req("Client_B", session)
        
        start_time = time.perf_counter()
        results = await asyncio.gather(task1, task2)
        end_time = time.perf_counter()
        
        total_time = end_time - start_time
        
        print("\n==== RESULTS =====")
        print(f"Time elapsed: {total_time:.4f}s")
        for idx, (successes, fails) in enumerate(results):
            client_name = "Client_A" if idx == 0 else "Client_B"
            print(f"{client_name}:")
            print(f"  Successes:  {successes}/{100}")
            print(f"  Fails:      {fails}/{100}")

if __name__ == "__main__":
    asyncio.run(run())