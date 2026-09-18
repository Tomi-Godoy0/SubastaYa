import asyncio
import sys
import httpx


# ==============================
# CONFIGURACIÓN
# ==============================

BASE_URL = "https://localhost:7204"

AUCTION_ID = 3
BUYER_ID = 2
BID_AMOUNT = 6000

BID_ENDPOINT = f"{BASE_URL}/api/auctions/{AUCTION_ID}/bids"


# ==============================
# ENVÍO DE PUJA
# ==============================

async def send_bid(client: httpx.AsyncClient, request_number: int):
    payload = {
        "auctionId": AUCTION_ID,
        "buyerId": BUYER_ID,
        "amount": BID_AMOUNT
    }

    try:
        response = await client.post(
            BID_ENDPOINT,
            json=payload
        )

        print(
            f"[Request {request_number}] "
            f"HTTP {response.status_code} - {response.text}"
        )

        return response.status_code

    except Exception as ex:
        print(
            f"[Request {request_number}] "
            f"ERROR: {ex}"
        )

        return None


# ==============================
# TEST DE CONCURRENCIA
# ==============================

async def main():

    print("======================================")
    print("      TEST DE CONCURRENCIA")
    print("======================================")
    print(f"Endpoint: {BID_ENDPOINT}")
    print(f"BuyerId: {BUYER_ID}")
    print(f"Amount: {BID_AMOUNT}")
    print()

    async with httpx.AsyncClient(verify=False) as client:

        # Evento utilizado para liberar las dos peticiones
        # prácticamente al mismo tiempo.
        start_event = asyncio.Event()

        async def concurrent_bid(request_number: int):

            await start_event.wait()

            return await send_bid(
                client,
                request_number
            )

        task1 = asyncio.create_task(
            concurrent_bid(1)
        )

        task2 = asyncio.create_task(
            concurrent_bid(2)
        )

        # Liberar ambas solicitudes
        # de forma concurrente.
        start_event.set()

        results = await asyncio.gather(
            task1,
            task2
        )

    print()
    print("======================================")
    print("RESULTADO")
    print("======================================")

    successful = results.count(201)
    conflicts = results.count(409)

    print(f"HTTP 201: {successful}")
    print(f"HTTP 409: {conflicts}")
    print()

    # ==================================
    # VALIDACIÓN
    # ==================================

    if successful == 1 and conflicts == 1:

        print(
            "PASS: La concurrencia optimista "
            "funcionó correctamente."
        )

        print(
            "Una puja fue aceptada y la otra "
            "fue rechazada con HTTP 409 Conflict."
        )

        return 0

    print(
        "FAIL: El resultado no coincide con "
        "la concurrencia esperada."
    )

    print(
        "Se esperaba exactamente una respuesta "
        "201 y una respuesta 409."
    )

    return 1


if __name__ == "__main__":
    exit_code = asyncio.run(main())
    sys.exit(exit_code)