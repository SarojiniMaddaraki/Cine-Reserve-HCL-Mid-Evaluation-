import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.html',
  styleUrls: ['./home.css']
})
export class Home {

  activeFilter = 'All';
  genres = ['All', 'Action', 'Sci-Fi', 'Thriller', 'Drama', 'Adventure'];

  movies = [
    { id: 1, title: 'Avengers: Endgame', genre: 'Action', rating: 8.4, duration: '181 min', language: 'English', shows: ['10:00 AM', '2:00 PM', '6:00 PM', '9:30 PM'], poster: 'https://image.tmdb.org/t/p/w500/or06FN3Dka5tukK1e9sl16pB3iy.jpg', badge: 'BLOCKBUSTER' },
    { id: 2, title: 'Inception', genre: 'Sci-Fi', rating: 8.8, duration: '148 min', language: 'English', shows: ['11:00 AM', '3:00 PM', '7:00 PM'], poster: 'https://image.tmdb.org/t/p/w500/9gk7adHYeDvHkCSEqAvQNLV5Uge.jpg', badge: 'CULT CLASSIC' },
    { id: 3, title: 'Interstellar', genre: 'Sci-Fi', rating: 8.6, duration: '169 min', language: 'English', shows: ['9:00 AM', '1:00 PM', '5:00 PM', '9:00 PM'], poster: 'https://image.tmdb.org/t/p/w500/gEU2QniE6E77NI6lCU6MxlNBvIx.jpg', badge: 'EPIC' },
    { id: 4, title: 'The Dark Knight', genre: 'Action', rating: 9.0, duration: '152 min', language: 'English', shows: ['10:30 AM', '2:30 PM', '7:30 PM'], poster: 'https://image.tmdb.org/t/p/w500/qJ2tW6WMUDux911r6m7haRef0WH.jpg', badge: 'LEGENDARY' },
    { id: 5, title: 'Parasite', genre: 'Thriller', rating: 8.5, duration: '132 min', language: 'Korean', shows: ['12:00 PM', '4:00 PM', '8:00 PM'], poster: 'https://image.tmdb.org/t/p/w500/7IiTTgloJzvGI1TAYymCfbfl3vT.jpg', badge: 'OSCAR WINNER' },
    { id: 6, title: 'Dune: Part Two', genre: 'Sci-Fi', rating: 8.7, duration: '166 min', language: 'English', shows: ['11:30 AM', '3:30 PM', '7:30 PM', '10:00 PM'], poster: 'https://image.tmdb.org/t/p/w500/1pdfLvkbY9ohJlCjQH2CZjjYVvJ.jpg', badge: 'NEW' },
    { id: 7, title: 'Oppenheimer', genre: 'Drama', rating: 8.9, duration: '181 min', language: 'English', shows: ['10:00 AM', '2:00 PM', '6:30 PM'], poster: 'https://image.tmdb.org/t/p/w500/8Gxv8gSFCU0XGDykEGv7zR1n2ua.jpg', badge: 'OSCAR WINNER' },
    { id: 8, title: 'Avatar: The Way of Water', genre: 'Adventure', rating: 7.6, duration: '192 min', language: 'English', shows: ['11:00 AM', '3:00 PM', '8:00 PM'], poster: 'https://image.tmdb.org/t/p/w500/t6HIqrRAclMCA60NsSmeqe9oDkO.jpg', badge: 'VISUAL MARVEL' },
    { id: 9, title: 'No Time to Die', genre: 'Action', rating: 7.3, duration: '163 min', language: 'English', shows: ['12:30 PM', '4:30 PM', '8:30 PM'], poster: 'https://image.tmdb.org/t/p/w500/iUgygt3fscRoKWCV1d0C7FbM9TP.jpg', badge: '' },
    { id: 10, title: 'The Shawshank Redemption', genre: 'Drama', rating: 9.3, duration: '142 min', language: 'English', shows: ['1:00 PM', '5:00 PM', '9:00 PM'], poster: 'https://image.tmdb.org/t/p/w500/lyQBXzOQSuE59IsHyhrp0qIiPAz.jpg', badge: 'ALL TIME BEST' },
    { id: 11, title: 'Mad Max: Fury Road', genre: 'Action', rating: 8.1, duration: '120 min', language: 'English', shows: ['10:00 AM', '3:00 PM', '7:00 PM'], poster: 'https://image.tmdb.org/t/p/w500/8tZYtuWezp8JbcsvHYO0O46tFbo.jpg', badge: '' },
    { id: 12, title: 'Alien: Romulus', genre: 'Thriller', rating: 7.4, duration: '119 min', language: 'English', shows: ['2:00 PM', '6:00 PM', '10:00 PM'], poster: 'https://image.tmdb.org/t/p/w500/b33nnKl1GSFbao4l3fZDDqsMx0F.jpg', badge: 'NEW' },
  ];

  selectedShowtimes: { [movieId: number]: string } = {};

  constructor(private router: Router) {}

  get filteredMovies() {
    if (this.activeFilter === 'All') return this.movies;
    return this.movies.filter(m => m.genre === this.activeFilter);
  }

  setFilter(genre: string) {
    this.activeFilter = genre;
  }

  selectShowtime(movieId: number, time: string) {
    this.selectedShowtimes[movieId] = time;
  }

  bookMovie(movie: any) {
    this.router.navigate(['/booking'], {
      queryParams: {
        movieId: movie.id,
        movieTitle: movie.title,
        showtime: this.selectedShowtimes[movie.id] || movie.shows[0]
      }
    });
  }

  stars(rating: number): string {
    return '★'.repeat(Math.round(rating / 2)) + '☆'.repeat(5 - Math.round(rating / 2));
  }
}
