import threading
import requests
import time
import random
import urllib3

urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

BASE_URL = "https://localhost:7117/Reservation/api/reservations"

# hard to verify the number of 409's and 404's here so i'll only count the number of 500's

ENDPOINTS = [
    {"method": "GET", "url": f"{BASE_URL}/getall"},
    {"method": "GET", "url": f"{BASE_URL}/getmovieseats", "json": {"movieId": "ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1"}},
    {"method": "POST", "url": f"{BASE_URL}/post",
      "json": {"movieId": "ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1","movieName": "FNAF2","username": "Freddy Fazbear","seat": 7,"row": 6}},
    {"method": "PUT", "url": f"{BASE_URL}/put",
      "json": {"movieId": "ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1","movieName": "FNAF2","username": "Freddy Fazbear","seat": 7,"row": 6},
      "params": "William Afton"},
    {"method": "DELETE", "url": f"{BASE_URL}/delete",
     "json": {"movieId": "ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1","movieName": "FNAF2","username": "Freddy Fazbear","seat": 7,"row": 6}},
    {"method": "DELETE", "url": f"{BASE_URL}/delete",
     "json": {"movieId": "ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1","movieName": "FNAF2","username": "William Afton","seat": 7,"row": 6}},
]

def req(client_id, stats):
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
            response = requests.request(method, url, json=payload, params=params, verify=False)
            if response.status_code == 500:
                print(f"Client {client_id} FAILED {i}, response code {response.status_code} on {method} {url}")
                fails += 1
            else:
                successes += 1
        except Exception as e:
            print(f"Client {client_id} FAILED {i}, exception: {e}")
            fails += 1
    stats[client_id] = (successes, fails)

def run():
    print("\n==== TWO CLIENTS SPAMMING 100 RANDOM REQUESTS =====")
    start_time = time.perf_counter()
    stats = {"Client_A": (0, 0), "Client_B": (0, 0)}
    t1 = threading.Thread(target=req, args=("Client_A", stats))
    t2 = threading.Thread(target=req, args=("Client_B", stats))
    t1.start()
    t2.start()
    t1.join()
    t2.join()
    end_time = time.perf_counter()
    
    total_time = end_time - start_time
    
    print("\n==== RESULTS =====")
    print(f"Time elapsed: {total_time:.4f}s")
    for client_name, (successes, fails) in stats.items():
        print(f"{client_name}:")
        print(f"Successes:  {successes}/{100}")
        print(f"Fails:      {fails}/{100}")

if __name__ == "__main__":
    run()