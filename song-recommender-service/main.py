from fastapi import FastAPI
from pydantic import BaseModel
from typing import List, Optional
import json

app = FastAPI()
from fastapi.middleware.cors import CORSMiddleware

app = FastAPI()

origins = [
    "http://localhost:5000",
    "http://127.0.0.1:5000",
    "http://localhost",
    "*",
]

app.add_middleware(
    CORSMiddleware,
    allow_origins=origins,
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

with open('songs.json', 'r', encoding='utf-8') as f:
    songs_db = json.load(f)

class SongInput(BaseModel):
    title: str
    artist: Optional[str]
    genre: Optional[str]

class RecommendRequest(BaseModel):
    songs: List[SongInput]
    limit: int = 5

@app.post("/recommend")
def recommend(data: RecommendRequest):
    input_songs = data.songs
    limit = data.limit

    recommended = []

    input_artists = set(s.artist.lower() for s in input_songs if s.artist)
    artist_matches = [s for s in songs_db if s["Artist"].lower() in input_artists]

    for s in artist_matches:
        if len(recommended) >= limit:
            break
        if s not in recommended:
            recommended.append(s)

    if len(recommended) < limit:
        input_genres = set(s.genre.lower() for s in input_songs if s.genre)
        genre_matches = [s for s in songs_db if s["Genre"].lower() in input_genres and s not in recommended]

        for s in genre_matches:
            if len(recommended) >= limit:
                break
            recommended.append(s)

    if len(recommended) < limit:
        for s in songs_db:
            if s not in recommended:
                recommended.append(s)
            if len(recommended) >= limit:
                break

    return {"recommendations": recommended}
