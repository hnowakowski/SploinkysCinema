import time
import requests
import threading
import urllib3

urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

URL = "https://localhost:7117/Reservation/api/reservations/post"

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

def req(client_id, stats):
    successes = 0
    fails = 0
    seats = 0

    for i in range(100):
        if i % 20 == 0:
            print(f"Client {client_id} Iteration {i}")
        payload = PAYLOADS_A[i] if client_id == "Client_A" else PAYLOADS_B[i]
        try:
            response = requests.post(URL, json=payload, verify=False)
            if response.status_code == 500:
                print(f"Client {client_id} failed {i}, response code {response.status_code}")
                fails += 1
            else:
                successes += 1
                if response.status_code == 200:
                    seats += 1
        except Exception as e:
            print(f"Client {client_id} failed {i}, exception: {e}")
            fails += 1
    stats[client_id] = (successes, fails, seats)

def run():
    print("\n==== WILLIAM AFTON VS. FREDDY FAZBEAR =====")
    start_time = time.perf_counter()
    stats = {"Client_A": (0, 0, 0), "Client_B": (0, 0, 0)}
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
    for client_name, (successes, fails, seats) in stats.items():
        print(f"{client_name}:")
        print(f"Successes:  {successes}/{100}")
        print(f"Fails:      {fails}/{100}")
        print(f"Seats:      {seats}/{100}")

if __name__ == "__main__":
    run()