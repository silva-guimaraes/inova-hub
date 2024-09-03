package server

import (
	"database/sql"
	"encoding/json"
	"fmt"
	"log"
	"net/http"
	"strings"

	"inova-hub/cmd/web"
	"inova-hub/internal/database"

	"github.com/a-h/templ"
	"github.com/google/uuid"
)

func (s *Server) RegisterRoutes() http.Handler {

	mux := http.NewServeMux()
	fileServer := http.FileServer(http.FS(web.Files))

	mux.Handle(     "/assets/",     fileServer)

    // registrar metodos de Server como funcoes de callback.
    // esses metodos sao capazes de acessar Server como se estivessem sendo chamados pelo objeto original, oque 
    // inclui acessar o banco de dados.
	mux.HandleFunc( "/",            s.index)
	mux.HandleFunc( "/health",      s.healthHandler)
	mux.HandleFunc( "/i/{uuid}",    s.detailedIdea)

    mux.HandleFunc( "/u/{user}",    s.auth(
        func(w http.ResponseWriter, r *http.Request) {
            fmt.Fprintln(w, "authorized")
        }))

	mux.HandleFunc( "POST /reg",    s.register)
	mux.HandleFunc( "POST /login",  s.login)
	mux.HandleFunc( "GET /reg",     s.registerView)
	mux.HandleFunc( "GET /login",   s.loginView)

    return Logging(mux)
}

func (s *Server) userFromCookie(r *http.Request) *database.User {
    id, err := r.Cookie("session_id")
    if err != nil {
        return nil
    }
    user, ok := s.sessions[id.Value]
    if !ok {
        return nil
    }
    return user
}

func (s *Server) loginView(w http.ResponseWriter, r *http.Request) {
    user := s.userFromCookie(r)
    templ.Handler(web.Login(user)).ServeHTTP(w, r)
}
func (s *Server) registerView(w http.ResponseWriter, r *http.Request) {
    user := s.userFromCookie(r)
    templ.Handler(web.Register(user)).ServeHTTP(w, r)
}
func (s *Server) detailedIdea(w http.ResponseWriter, r *http.Request) {
    user := s.userFromCookie(r)
    templ.Handler(web.DetailedIdeia(user)).ServeHTTP(w, r)
}

func (s *Server) index(w http.ResponseWriter, r *http.Request) {
    if (r.URL.Path != "/") {
        http.NotFound(w, r)
        return
    }
    user := s.userFromCookie(r)
    templ.Handler(web.Index(user)).ServeHTTP(w, r)
}

// helper pra verificar se campo do form não esta vazio
func FormValue(r *http.Request, formKey string) (string, error){
    value := strings.TrimSpace(r.FormValue(formKey))
    if value == "" {
        return "", fmt.Errorf("not found: %s", formKey)
    }
    return value, nil
}

func (s *Server) register(w http.ResponseWriter, r *http.Request) {
    email,      err1 := FormValue(r, "email")
    senha,      err2 := FormValue(r, "password")
    username,   err3 := FormValue(r, "username")
    if err1 != nil || err2 != nil || err3 != nil {
        http.Error(w, http.StatusText(400), 400)
        fmt.Println(err1, err2, err3)
        return
    }

    err := s.db.RegisterUser(username, email, senha)
    if err != nil {
        http.Error(w, http.StatusText(500), 500)
        fmt.Println(err)
        return
    }

    // log.Println("usuário registrado!", username, email, senha)
    // s.setNewSessionId(w)

    http.Redirect(w, r, "/" + username, http.StatusFound)
}

func (s *Server) setNewSessionId(w http.ResponseWriter, user *database.User) {
    id := uuid.NewString() // pode dar pânico!

    http.SetCookie(w, &http.Cookie{
        Name: "session_id",
        Value: id,
        Path: "/",
        MaxAge: 0, // infinito
    })
    s.sessions[id] = user
} 

func (s *Server) login(w http.ResponseWriter, r *http.Request) {
    email, err1 := FormValue(r, "email")
    senha, err2 := FormValue(r, "password")
    if err1 != nil || err2 != nil {
        http.Error(w, http.StatusText(400), 400)
        log.Panic(err1, err2)
    }

    user, err := s.db.FirstUser(email, senha)
    if err == sql.ErrNoRows {
        http.NotFound(w, r)
        log.Panic(err)
        return

    } else if err != nil {
        http.Error(w, http.StatusText(500), 500)
        log.Panic(err)
        return
    }

    s.setNewSessionId(w, user)

    http.Redirect(w, r, fmt.Sprintf("/u/%s", user.Username), http.StatusFound)
}

func (s *Server) healthHandler(w http.ResponseWriter, r *http.Request) {
	jsonResp, err := json.Marshal(s.db.Health())

	if err != nil {
		log.Fatalf("error handling JSON marshal. Err: %v", err)
	}

	_, _ = w.Write(jsonResp)
}

// // autenticá usuário para uma rota em específico.
// // caso seja que o usuário está presente no banco, essa rota irá chamar o proximo handler que lhe foi
// // passado como argumento.
func (s *Server) auth(next http.HandlerFunc) http.HandlerFunc {
    return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
        id, err := r.Cookie("session_id")
        if err != nil {
            http.Error(w, "Unauthorized", http.StatusUnauthorized)
            log.Println(err)
            return
        }
        if _, ok := s.sessions[id.Value]; !ok {
            http.Error(w, "Unauthorized", http.StatusUnauthorized)
            log.Println(err)
            return
        }

        next.ServeHTTP(w, r)
    })
}
