#!/bin/bash

AUCTION_ID=3
AMOUNT=13000
URL="https://localhost:7204/api/auctions/$AUCTION_ID/bids"

rm -rf resultados
mkdir resultados

for i in $(seq 1 100)
do
  BUYER_ID=$((2 + i % 2))
  curl -k -X POST "$URL" \
    -H "Content-Type: application/json" \
    -d "{\"auctionId\": $AUCTION_ID, \"buyerId\": $BUYER_ID, \"amount\": $((AMOUNT + i))}" \
    -o /dev/null -s -w "Request $i (Buyer $BUYER_ID): %{http_code}\n" > "resultados/req_$i.txt" &
done

wait

echo "=== Resultados ==="
cat resultados/req_*.txt | sort -t' ' -k2 -n

echo ""
echo "=== Resumen ==="
echo "201 (exitosas): $(grep -l ": 201" resultados/*.txt | wc -l)"
echo "409 (conflicto de concurrencia): $(grep -l ": 409" resultados/*.txt | wc -l)"
echo "400 (validación de negocio): $(grep -l ": 400" resultados/*.txt | wc -l)"
echo "422 (fondos insuficientes): $(grep -l ": 422" resultados/*.txt | wc -l)"