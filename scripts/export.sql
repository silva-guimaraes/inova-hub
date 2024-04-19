--
-- PostgreSQL database dump
--

-- Dumped from database version 16.2 (Debian 16.2-1.pgdg120+2)
-- Dumped by pg_dump version 16.2

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Data for Name: user; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."user" VALUES (1, 'daniel', 'silva__guimaraes@hotmail.com', 'enka speed');
INSERT INTO public."user" VALUES (2, 'henrique', 'henrique@email.com', 'henrique_senha');
INSERT INTO public."user" VALUES (3, 'raphael', 'raphael@email.com', 'raphael_senha');
INSERT INTO public."user" VALUES (4, 'breno', 'breno@email.com', 'breno_senha');


--
-- Data for Name: idea; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.idea VALUES (1, 'Inova Hub', 4, 'O Inova Hub é uma plataforma de investimento de idéias');
INSERT INTO public.idea VALUES (2, 'Duolingo', 3, 'Duoligo é um aplicativo para aprendizado de linguagens');
INSERT INTO public.idea VALUES (3, 'Ifood', 2, 'iFood é um apilcativo que procura facilitar entregas de lanches');


--
-- Data for Name: post; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: comment; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: favorite; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: image; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: upvote; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.upvote VALUES (1, 1, '2024-04-09');
INSERT INTO public.upvote VALUES (1, 2, '2024-04-11');
INSERT INTO public.upvote VALUES (1, 3, '2024-04-11');
INSERT INTO public.upvote VALUES (2, 2, '2024-04-11');
INSERT INTO public.upvote VALUES (3, 2, '2024-04-11');


--
-- Name: comment_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.comment_id_seq', 1, false);


--
-- Name: idea_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.idea_id_seq', 3, true);


--
-- Name: post_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.post_id_seq', 1, false);


--
-- Name: user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.user_id_seq', 4, true);


--
-- PostgreSQL database dump complete
--

