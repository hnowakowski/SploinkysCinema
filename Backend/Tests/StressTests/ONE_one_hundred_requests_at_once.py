import aiohttp
import asyncio
import time

URL = "https://localhost:7117/Reservation/api/reservations/getall"
EXPECTED_RESPONSE = sorted([
  {
    "movieId": "ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1",
    "movieName": "FNAF2",
    "username": "Jan Kowalski",
    "seat": 1,
    "row": 3,
    "lastUpdate": "2026-06-11T14:59:32.809"
  },
  {
    "movieId": "ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1",
    "movieName": "FNAF2",
    "username": "Maciej Piernik",
    "seat": 2,
    "row": 4,
    "lastUpdate": "2026-06-11T14:59:32.823"
  },
  {
    "movieId": "ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1",
    "movieName": "FNAF2",
    "username": "Krzysztof Martyn",
    "seat": 3,
    "row": 3,
    "lastUpdate": "2026-06-11T14:59:32.82"
  },
  {
    "movieId": "ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1",
    "movieName": "FNAF2",
    "username": "Miłosz Kadziński",
    "seat": 4,
    "row": 2,
    "lastUpdate": "2026-06-11T14:59:32.813"
  },
  {
    "movieId": "2e35f12b-7dcd-46ee-a141-595f031d30eb",
    "movieName": "Minecraft",
    "username": "Miłosz Kadziński",
    "seat": 1,
    "row": 2,
    "lastUpdate": "2026-06-11T14:59:32.827"
  },
  {
    "movieId": "2e35f12b-7dcd-46ee-a141-595f031d30eb",
    "movieName": "Minecraft",
    "username": "Maciej Piernik",
    "seat": 2,
    "row": 2,
    "lastUpdate": "2026-06-11T14:59:32.832"
  },
  {
    "movieId": "2e35f12b-7dcd-46ee-a141-595f031d30eb",
    "movieName": "Minecraft",
    "username": "Krzysztof Martyn",
    "seat": 3,
    "row": 2,
    "lastUpdate": "2026-06-11T14:59:32.83"
  },
  {
    "movieId": "5ff94089-6b0b-4e6b-85b2-1dccd568f60c",
    "movieName": "Iron Lung",
    "username": "Miłosz Kadziński",
    "seat": 1,
    "row": 3,
    "lastUpdate": "2026-06-11T14:59:32.835"
  },
  {
    "movieId": "5ff94089-6b0b-4e6b-85b2-1dccd568f60c",
    "movieName": "Iron Lung",
    "username": "Maciej Piernik",
    "seat": 3,
    "row": 2,
    "lastUpdate": "2026-06-11T14:59:32.839"
  }
], key=lambda x: (x['movieId'], x['username'], x['seat'], x['row']))

async def fetch(session, idx):
    try:
        async with session.get(URL, ssl=False) as response:
            if response.status == 200:
                res = await response.json()
                res = sorted(res, key=lambda x: (x['movieId'], x['username'], x['seat'], x['row']))
                # print(res, "\n===\n", EXPECTED_RESPONSE)
                if res == EXPECTED_RESPONSE:
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

async def run():
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
    asyncio.run(run())