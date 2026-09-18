AUCTION_ID=3
BUYER_1=2
BUYER_2=3
AMOUNT=9000
URL="https://localhost:7204/api/auctions/$AUCTION_ID/bids"

curl -k -X POST "$URL" \
  -H "Content-Type: application/json" \
  -d "{\"auctionId\": $AUCTION_ID, \"buyerId\": $BUYER_1, \"amount\": $AMOUNT}" \
  -w "\n[Petici�n 1 - Buyer $BUYER_1] Status: %{http_code}\n" &

curl -k -X POST "$URL" \
  -H "Content-Type: application/json" \
  -d "{\"auctionId\": $AUCTION_ID, \"buyerId\": $BUYER_2, \"amount\": $AMOUNT}" \
  -w "\n[Petici�n 2 - Buyer $BUYER_2] Status: %{http_code}\n" &

wait
echo "Ambas peticiones completadas"