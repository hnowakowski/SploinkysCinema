import aiohttp
import asyncio
import time

URL = "https://localhost:7117/Reservation/api/reservations/getall" # get it from yaml later
RESPONSE_JSON = [ # come up with a better way to do it later
  {
    "id": "026f098b-1f6a-4833-bab3-8d8e00ccc529",
    "name": "Maciej",
    "surname": "Piernik",
    "playId": 0,
    "seat": 4,
    "row": 1
  },
  {
    "id": "075e1650-bbf4-4f0c-87ef-a562fd8bdaa1",
    "name": "Krzysztof",
    "surname": "Martyn",
    "playId": 0,
    "seat": 3,
    "row": 1
  },
  {
    "id": "f5d316df-8fd9-42ec-ab4d-aaf76cdb7039",
    "name": "Maciej",
    "surname": "Piernik",
    "playId": 0,
    "seat": 4,
    "row": 1
  },
  {
    "id": "8e82f6c5-ee38-419d-8492-58a617a4a5aa",
    "name": "Jan",
    "surname": "Kowalski",
    "playId": 0,
    "seat": 1,
    "row": 1
  },
  {
    "id": "0deb93fe-4a74-4c39-8c90-fd39abeb35d2",
    "name": "Krzysztof",
    "surname": "Martyn",
    "playId": 0,
    "seat": 3,
    "row": 1
  },
  {
    "id": "82f4ab52-4aed-42f2-9e94-3c3e9b3e8575",
    "name": "Jan",
    "surname": "Kowalski",
    "playId": 0,
    "seat": 1,
    "row": 1
  },
  {
    "id": "63a3de83-ab18-4e20-8acd-f34962a8cb19",
    "name": "Miłosz",
    "surname": "Kadziński",
    "playId": 0,
    "seat": 2,
    "row": 1
  },
  {
    "id": "168748a4-2b03-4bd4-adab-38c6d61653e5",
    "name": "Miłosz",
    "surname": "Kadziński",
    "playId": 0,
    "seat": 2,
    "row": 1
  }
]

async def fetch(session, idx):
    try:
        async with session.get(URL, ssl=False) as response:
            if response.status == 200:
                if await response.json() == RESPONSE_JSON:
                    return True
                else:
                    print(f"FAILED {idx}, response content mismatch")
                    return False
            else:
                print(f"FAILED {idx}, response code {response.status}")
                return False
    except Exception as e:
        print(f"FAILED {idx}, exception: {e}")
        return False

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