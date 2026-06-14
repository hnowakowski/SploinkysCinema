import aiohttp
import asyncio
import time
import requests
from datetime import datetime, timezone

URL = "https://localhost:7117/Reservation/api/reservations/post"

# will count the 500's as fails and will count the 200's at the end to check how many seats each client got
# could do multithreading but asyncio also makes all of this get evaluated in non-deterministic order

PAYLOADS_A = []
PAYLOADS_B = []

for i in range(100):
    PAYLOADS_A.append(
        {
            "movieId": "ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1",
            "movieName": "FNAF2",
            "username": "Freddy Fazbear",
            "seat": (i % 10) + 1,
            "row": (i // 10) + 1
        })
    
for i in range(100):
    PAYLOADS_B.append(
        {
            "movieId": "ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1",
            "movieName": "FNAF2",
            "username": "William Afton",
            "seat": (i % 10) + 1,
            "row": (i // 10) + 1
        })

async def req(client_id, session):
    successes = 0
    fails = 0
    seats = 0
    
    for i in range(100):
        if i % 20 == 0:
            print(f"Client {client_id} Iteration {i}")
        payload = PAYLOADS_A[i] if client_id == "Client_A" else PAYLOADS_B[i]
        try:
            async with session.request("POST", URL, json=payload, ssl=False) as response:
                if response.status == 500:
                    print(f"Client {client_id} FAILED {i}, response code {response.status}")
                    fails += 1
                else:
                    successes += 1
                    if response.status == 200:
                        seats += 1
        except Exception as e:
            print(f"Client {client_id} FAILED {i}, exception: {e}")
            fails += 1
    return successes, fails, seats

async def run():
    print("\n==== WILLIAM AFTON VS. FREDDY FAZBEAR =====")
    start_time = time.perf_counter()
    stats = {"Client_A": {"successes": 0, "fails": 0, "seats": 0}, "Client_B": {"successes": 0, "fails": 0, "seats": 0}}
    
    async with aiohttp.ClientSession() as session:
            # i think client_a gets invoked first so might need to run it a few times to get a more even distribution, but it's rarely 100-0
            results = await asyncio.gather(req("Client_A", session), req("Client_B", session))
            
            for id, (successes, fails, seats) in enumerate(results):
                client_name = "Client_A" if id == 0 else "Client_B"
                stats[client_name]["successes"] = successes
                stats[client_name]["fails"] = fails
                stats[client_name]["seats"] = seats
                stats[client_name]["seats"] = seats
                        
    end_time = time.perf_counter()
    total_time = end_time - start_time
    
    print("\n==== RESULTS =====")
    print(f"Time elapsed: {total_time:.4f}s")
    for client_name, data in stats.items():
        print(f"{client_name}:")
        print(f"Successes:  {data['successes']}/{100}")
        print(f"Fails:      {data['fails']}/{100}")
        print(f"Seats:      {data['seats']}/{100}")

if __name__ == "__main__":
    asyncio.run(run())