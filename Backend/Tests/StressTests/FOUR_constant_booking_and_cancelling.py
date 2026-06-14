import aiohttp
import asyncio
import time
import requests
from datetime import datetime, timezone
import urllib3

urllib3.disable_warnings()

REQUESTS = [
    {"url": "https://localhost:7117/Reservation/api/reservations/post",
      "json": {"movieId": "ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1","movieName": "FNAF2","username": "Freddy Fazbear","seat": 7,"row": 6}},
    {"url": "https://localhost:7117/Reservation/api/reservations/delete",
     "json": {"movieId": "ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1","movieName": "FNAF2","username": "Freddy Fazbear","seat": 7,"row": 6}}
]

def run():
    print("\n==== MR. FAZBEAR IS UNDECIDED =====")
    start_time = time.perf_counter()
    results = []
    for i in range(100):
        if i % 10 == 0:
            print(f"Iteration {i}")
        try:
            if i % 2 == 0:
                response = requests.post(REQUESTS[0]["url"], json=REQUESTS[0]["json"], verify=False)
            else:
                response = requests.delete(REQUESTS[1]["url"], json=REQUESTS[1]["json"], verify=False)
            if response.status_code == 200:
                results.append(True)
            else:
                print(f"FAILED {i}, status code: {response.status_code}")
                results.append(False)
        except Exception as e:
            print(f"FAILED {i}, exception: {e}")
            results.append(False)
    end_time = time.perf_counter()

    successes = results.count(True)
    fails = results.count(False)
    total_time = end_time - start_time
        
    print("\n==== RESULTS =====")
    print(f"Time elapsed: {total_time:.4f}s")
    print(f"Successes:    {successes}/100")
    print(f"Fails:        {fails}/100")

if __name__ == "__main__":
    run()