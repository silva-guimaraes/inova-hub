package main

import (
    "fmt"
    "inova-hub/internal/server"
)

func main() {

    s := server.NewServer()

    fmt.Printf("http://localhost%s\n", s.Addr)
    err := s.ListenAndServe()
    if err != nil {
        panic(fmt.Sprintf("cannot start server: %s", err))
    }
}
