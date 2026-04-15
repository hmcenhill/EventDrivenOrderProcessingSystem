## Local infrastructure

This project uses Docker Compose for local development infrastructure.

### Services

- PostgreSQL: `localhost:5432`
- RabbitMQ AMQP: `localhost:5672`
- RabbitMQ Management UI: `http://localhost:15672`
- Redis: `localhost:6379`

### Default credentials

#### PostgreSQL

- Database: `orderprocessing`
- Username: `postgres`
- Password: `postgres`

#### RabbitMQ

- Username: `guest`
- Password: `guest`

### Start services

```bash
docker compose up -d

```
