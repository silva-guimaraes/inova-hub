package database

import (
	"context"
	"database/sql"
	"fmt"
	"log"
	"os"
	"strconv"
	"time"

	_ "github.com/joho/godotenv/autoload"
  _ "github.com/jackc/pgx/v5/stdlib"
)


// Models:
type User struct {
    Username string
    Password_hash string
    Email string
}

// // Service represents a service that interacts with a database.
// type Service interface {
// 	// Health returns a map of health status information.
// 	// The keys and values in the map are Service-specific.
// 	Health() map[string]string
//
//     FirstUser(email, password string) (*User, error) 
//     RegisterUser(username, email, password_hash string) error
//
// 	// Close terminates the database connection.
// 	// It returns an error if the connection cannot be closed.
// 	Close() error
// }

// wrapper que esconde todas as funções do banco de dados nesse arquivo
type Service struct {
	db *sql.DB
}

var (
	dburl      = os.Getenv("DB_URL")
	dbInstance *Service
)

func New() *Service {
    log.Println("new?!")
	// Reuse Connection
	if dbInstance != nil {
		return dbInstance
	}

	db, err := sql.Open("pgx", dburl)
	if err != nil {
		// This will not be a connection error, but a DSN parse error or
		// another initialization error.
		log.Fatal(err)
	}

	dbInstance = &Service{
		db: db,
	}
	return dbInstance
}


// retorna o primeiro usario encontrado que bate com os argumentos fornecidos.
// caso usuário nao tenha sido encontrado retorna `sql.ErrNoRows`
func (s *Service) FirstUser(email, password_hash string) (*User, error){
    // QueryRow retorna o resultado o primeiro resultado mesmo que possivelmente vazio
    query := `SELECT    username,    email,    password_hash FROM "user" WHERE email = $1 AND password_hash = $2 LIMIT 1`
    row := s.db.QueryRow(query, email, password_hash)
    u := new(User)
    err := row.Scan( &u.Username, &u.Email, &u.Password_hash) // retorna erro caso nenhuma linha tenha sido retornada
    if err != nil {
        return nil, err
    }
    return u, nil
}

// adiciona novo usuário ao banco. usado na tela de registro.
func (s *Service) RegisterUser(username, email, password_hash string) error{
    query := `insert into "user" (username, email, password_hash) values ($1, $2, $3)`
    _, err := s.db.Exec(query , username, email, password_hash)
    if err != nil {
        return err
    }
    return nil
}

// esses foram gerados junto com o scaffolding usado no inicio do projeto:

// Health checks the health of the database connection by pinging the database.
// It returns a map with keys indicating various health statistics.
func (s *Service) Health() map[string]string {
	ctx, cancel := context.WithTimeout(context.Background(), 1*time.Second)
	defer cancel()

	stats := make(map[string]string)

	// Ping the database
	err := s.db.PingContext(ctx)
	if err != nil {
		stats["status"] = "down"
		stats["error"] = fmt.Sprintf("db down: %v", err)
		log.Fatalf(fmt.Sprintf("db down: %v", err)) // Log the error and terminate the program
		return stats
	}

	// Database is up, add more statistics
	stats["status"] = "up"
	stats["message"] = "It's healthy"

	// Get database stats (like open connections, in use, idle, etc.)
	dbStats := s.db.Stats()
	stats["open_connections"] = strconv.Itoa(dbStats.OpenConnections)
	stats["in_use"] = strconv.Itoa(dbStats.InUse)
	stats["idle"] = strconv.Itoa(dbStats.Idle)
	stats["wait_count"] = strconv.FormatInt(dbStats.WaitCount, 10)
	stats["wait_duration"] = dbStats.WaitDuration.String()
	stats["max_idle_closed"] = strconv.FormatInt(dbStats.MaxIdleClosed, 10)
	stats["max_lifetime_closed"] = strconv.FormatInt(dbStats.MaxLifetimeClosed, 10)

	// Evaluate stats to provide a health message
	if dbStats.OpenConnections > 40 { // Assuming 50 is the max for this example
		stats["message"] = "The database is experiencing heavy load."
	}

	if dbStats.WaitCount > 1000 {
		stats["message"] = "The database has a high number of wait events, indicating potential bottlenecks."
	}

	if dbStats.MaxIdleClosed > int64(dbStats.OpenConnections)/2 {
		stats["message"] = "Many idle connections are being closed, consider revising the " +
        "connection pool settings."
	}

	if dbStats.MaxLifetimeClosed > int64(dbStats.OpenConnections)/2 {
		stats["message"] = "Many connections are being closed due to max lifetime, consider increasing max " +
        "lifetime or revising the connection usage pattern."
	}

	return stats
}

// Close closes the database connection.
// It logs a message indicating the disconnection from the specific database.
// If the connection is successfully closed, it returns nil.
// If an error occurs while closing the connection, it returns the error.
func (s *Service) Close() error {
	log.Printf("Disconnected from database: %s", dburl)
	return s.db.Close()
}
