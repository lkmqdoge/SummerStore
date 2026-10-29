run-clean:
    docker compose build --no-cache && docker compose up -d

run:
    docker compose up -d

stop:
    docker compose down

