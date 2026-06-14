import requests
import time
import urllib3
from datetime import datetime, timezone
from ZERO_tons_of_data import run as insert_run

urllib3.disable_warnings()

URL = "https://localhost:7117/Reservation/api/reservations/deleteall"

# Book an entire theater for FNAF2, so 10x10, then nuke it all
PARAMS = {"movieId": "ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1", "username": "Freddy Fazbear"}

def run():
    insert_run()
    print("\n==== FREDDY FAZBEAR NUKES THE ENTIRE THEATER =====")
    fail = False
    start_time = time.perf_counter()
    try:
        response = requests.delete(URL, params=PARAMS, verify=False)
        if response.status_code != 200:
            print(f"FAILED, status code: {response.status_code}")
            fail = True
    except Exception as e:
        print(f"FAILED, exception: {e}")
        fail = True
    end_time = time.perf_counter()

    total_time = end_time - start_time
    status = "SUCCESS" if not fail else "FAILURE"

    print("\n==== THE CONSEQUENCES OF MR. FAZBEAR'S ACTIONS =====")
    print(f"Time elapsed: {total_time:.4f}s")
    print(f"Status:      {status}")

if __name__ == "__main__":
    run()

