import requests
import time
import urllib3
from datetime import datetime, timezone

urllib3.disable_warnings()

URL = "https://localhost:7117/Reservation/api/reservations/post"

PAYLOADS = []
# Book an entire theater for FNAF2, so 10x10
# There are a few taken seats in the mock schema, so there should be 4 409's and 96 200's
# (as a fun bonus this also checks if the seat is already taken)
for i in range(100):
    PAYLOADS.append(
        {
            "movieId": "ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1",
            "movieName": "FNAF2",
            "username": "Freddy Fazbear",
            "seat": (i % 10) + 1,
            "row": (i // 10) + 1,
            "lastUpdate": datetime.now(timezone.utc).isoformat()
        })
    
def run():
    print("\n==== FREDDY FAZBEAR BOOKS THE ENTIRE THEATER =====")
    start_time = time.perf_counter()
    results = []
    hit_409 = 0
    hit_200 = 0
    for i, payload in enumerate(PAYLOADS):
        if i % 10 == 0:
            print(f"Iteration {i}")
        try:
            response = requests.post(URL, json=payload, verify=False)
            if hit_200 <= 96 and response.status_code == 200:
                results.append(True)
                hit_200 += 1
            elif 4 <= hit_409 and response.status_code == 409:
                results.append(True)
                hit_409 += 1
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
        
    print("\n==== THE CONSEQUENCES OF MR. FAZBEAR'S ACTIONS =====")
    print(f"Time elapsed: {total_time:.4f}s")
    print(f"Successes:    {successes}/100")
    print(f"Fails:        {fails}/100")

if __name__ == "__main__":
    run()

