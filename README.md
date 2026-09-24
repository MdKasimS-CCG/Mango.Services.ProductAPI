# Order API for Mango application.

# Running Docker via any terminal

## Building docker image
docker build -t mango-productapi-local:dev .

## Running container
docker run --name mango-productapi --env-file ".env"  -p 5156:8080 mango-productapi-local:dev
