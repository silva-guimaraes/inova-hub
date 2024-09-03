package server

import (
  "fmt"
  "net/http"
  "time"

  _ "github.com/joho/godotenv/autoload"

  "inova-hub/internal/database"
)

type Server struct {
  port int
  db *database.Service
  sessions map[string]*database.User
}

func NewServer() *http.Server {
  port := 2929
  NewServer := &Server{
    port: port,
    sessions: make(map[string]*database.User),
    db: database.New(),
  }

  // Declare Server config
  server := &http.Server{
    Addr:         fmt.Sprintf(":%d", NewServer.port),
    Handler:      NewServer.RegisterRoutes(),
    IdleTimeout:  time.Minute,
    ReadTimeout:  10 * time.Second,
    WriteTimeout: 30 * time.Second,
  }

  return server
}
