# import aiohttp
import requests
import time
import urllib3
from datetime import datetime, timezone

urllib3.disable_warnings()

URL = "https://localhost:7117/Reservation/api/reservations/post"
PAYLOAD = {
    "movieId": "ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1",
    "movieName": "FNAF2",
    "username": "Freddy Fazbear",
    "seat": 7,
    "row": 6
  }

# It should have one 200 and 99 409's
# error 500 counts as a fail

def run():
    print("\n==== SPAMMING ONE HUNDRED INSERTS =====")
    start_time = time.perf_counter()
    results = []
    hit_200 = False
    for i in range(100):
        if i % 10 == 0:
            print(f"Iteration {i}")
        try:
            response = requests.post(URL, json=PAYLOAD, verify=False)
            if hit_200 == False and response.status_code == 200:
                results.append(True)
                hit_200 = True
            elif hit_200 == True and response.status_code == 409:
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