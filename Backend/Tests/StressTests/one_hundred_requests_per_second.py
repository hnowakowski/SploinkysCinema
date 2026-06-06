import aiohttp
import asyncio
import time

URL = "https://localhost:7117/Reservation/api/reservations/getall" # get it from yaml later
EXPECTED_RESPONSE = sorted([
  {
    "name": "Jan",
    "surname": "Kowalski",
    "playId": 0,
    "seat": 1,
    "row": 1
  },
  {
    "name": "Krzysztof",
    "surname": "Martyn",
    "playId": 0,
    "seat": 3,
    "row": 1
  },
  {
    "name": "Miłosz",
    "surname": "Kadziński",
    "playId": 0,
    "seat": 2,
    "row": 1
  },
  {
    "name": "Maciej",
    "surname": "Piernik",
    "playId": 0,
    "seat": 4,
    "row": 1
  }
], key= lambda x: (x['name'], x['surname'], x['playId'], x['seat'], x['row']))

async def fetch(session, idx):
    try:
        async with session.get(URL, ssl=False) as response:
            if response.status == 200:
                response_json = await response.json()
                response_json = sorted(response_json, key=lambda x: (x['name'], x['surname'], x['playId'], x['seat'], x['row']))
                for id, item in enumerate(response_json):
                    for key, value in EXPECTED_RESPONSE[id].items():
                        if item.get(key) != value:
                            print(f"FAILED {idx}, response content mismatch at item {id}, key '{key}'")
                            print(f"Expected: {value}, Got: {item.get(key)}")
                            return False
            else:
                print(f"FAILED {idx}, response code {response.status}")
                return False
    except Exception as e:
        print(f"FAILED {idx}, exception: {e}")
        return False
    return True



async def one_hundred_requests():
    print("\n==== SENDING ONE HUNDRED REQUESTS AT ONCE =====")
    async with aiohttp.ClientSession() as session:
        tasks = [fetch(session, i) for i in range(100)]
        start_time = time.perf_counter()
        results = await asyncio.gather(*tasks)
        end_time = time.perf_counter()
        
        successes = results.count(True)
        fails = results.count(False)
        total_time = end_time - start_time
        
        print("\n==== ONE HUNDRED REQUESTS RESULTS =====")
        print(f"Time elapsed: {total_time:.4f}s")
        print(f"Successes:    {successes}/100")
        print(f"Fails:        {fails}/100")

if __name__ == "__main__":
    asyncio.run(one_hundred_requests())