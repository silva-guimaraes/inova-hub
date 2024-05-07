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
INSERT INTO public.idea VALUES (4, 'Airbnb', 2, 'Airbnb é um serviço online comunitário para as pessoas anunciarem, descobrirem e reservarem acomodações e meios de hospedagem.');
INSERT INTO public.idea VALUES (5, 'Uber', 2, 'Uber é uma empresa multinacional americana, prestadora de serviços eletrônicos na área do transporte privado urbano, através de um aplicativo de transporte que permite a busca por motoristas baseada na localização, em inglês e-hailing, oferecendo um serviço semelhante ao tradicional táxi. É conhecido popularmente como serviços de "carona remunerada".');
INSERT INTO public.idea VALUES (6, 'Google', 3, 'é uma empresa multinacional de softwares e serviços online (baseado na nuvem) fundada em 1998 na cidade norte-americana de Menlo Park (estado da Califórnia), que lucra principalmente através da publicidade pelo AdWords. A Google é a principal subsidiária da Alphabet Inc.');
INSERT INTO public.idea VALUES (7, 'Wikipedia', 3, 'A Wikipédia é um projeto de enciclopédia multilíngue de licença livre,[2][3] baseado na web e escrito de maneira colaborativa.[3] Foi lançado em 2001 por Jimmy Wales e Larry Sanger[4] e é atualmente administrado pela Fundação Wikimedia[5] (organização sem fins lucrativos que engaja pessoas para desenvolver conteúdo educacional sob uma licença livre ou no domínio público e para disseminá-lo globalmente),[6] integrando vários projetos[2] mantidos pela fundação. É formada por mais de 61 milhões de artigos (1 123 876 em português, até 4 de maio de 2024) escritos de forma conjunta por diversos editores voluntários ao redor do mundo. Em maio de 2023, havia edições ativas da Wikipédia em 321 idiomas.');
INSERT INTO public.idea VALUES (8, 'Apple', 2, 'Apple é uma empresa multinacional norte-americana que tem o objetivo de projetar e comercializar produtos eletrônicos de consumo, software de computador e computadores pessoais. Os produtos de hardware mais conhecidos da empresa incluem a linha de computadores Macintosh, iPod, iPhone, iPad, Apple TV e o Apple Watch. Os softwares incluem o sistema operacional macOS, o navegador de mídia iTunes, suíte de software multimídia e criatividade iLife, suíte de software de produtividade iWork, Aperture, um pacote de fotografia profissional, Final Cut Studio, uma suíte de vídeo profissional, produtos de software, Logic Studio, um conjunto de ferramentas de produção musical, navegador Safari e o iOS, um sistema operacional móvel.');
INSERT INTO public.idea VALUES (9, 'Mercedes', 2, 'A Mercedes-Benz é uma marca alemã de automóveis[1][2] pertencente a Mercedes-Benz Group criada em 1924, sendo resultado de uma fusão entre a Benz & Cie. e a Daimler-Motoren-Gesellschaft, é uma das mais antigas fabricantes de automóveis do mundo. Também produz caminhões, autocarros e os seus próprios motores.');


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

SELECT pg_catalog.setval('public.idea_id_seq', 9, true);


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

