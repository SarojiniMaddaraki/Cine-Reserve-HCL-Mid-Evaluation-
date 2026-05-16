import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-seat-booking',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './seat-booking.html',
  styleUrls: ['./seat-booking.css']
})
export class SeatBooking {

  seats: any[] = [];
  selectedSeats: any[] = [];
  total = 0;
  bookingSuccess = false;
  bookingRef = '';
  error = '';
  loading = false;

  movieTitle = 'Now Showing';
  showtime = '';

  readonly PRICE_PREMIUM = 200;
  readonly PRICE_STANDARD = 120;

  constructor(private http: HttpClient, private route: ActivatedRoute) {
    this.route.queryParams.subscribe(params => {
      this.movieTitle = params['movieTitle'] || 'Now Showing';
      this.showtime = params['showtime'] || '';
    });
    this.generateSeats();
  }

  generateSeats() {
    const rows = ['A','B','C','D','E','F','G','H'];
    rows.forEach(row => {
      for (let i = 1; i <= 10; i++) {
        const sold = Math.random() < 0.2;
        this.seats.push({ row, number: i, status: sold ? 'sold' : 'available' });
      }
    });
  }

  toggleSeat(seat: any) {
    if (seat.status === 'sold') return;
    if (seat.status === 'available') {
      seat.status = 'selected';
      this.selectedSeats.push(seat);
    } else {
      seat.status = 'available';
      this.selectedSeats = this.selectedSeats.filter(s => !(s.row === seat.row && s.number === seat.number));
    }
    this.calculateTotal();
  }

  calculateTotal() {
    this.total = this.selectedSeats.reduce((sum, s) =>
      sum + (s.row === 'G' || s.row === 'H' ? this.PRICE_PREMIUM : this.PRICE_STANDARD), 0
    );
  }

  isPremium(row: string) { return row === 'G' || row === 'H'; }

  get rows() {
    return ['A','B','C','D','E','F','G','H'];
  }

  seatsForRow(row: string) {
    return this.seats.filter(s => s.row === row);
  }

  getUserIdFromToken(token: string): number {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const key = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier';
      return parseInt(payload[key]) || 1;
    } catch {
      return 1;
    }
  }

  bookTickets() {
    if (!this.selectedSeats.length) return;
    this.loading = true;
    this.error = '';

    const token = localStorage.getItem('token');

    if (!token) {
      this.error = 'You are not logged in. Please login first.';
      this.loading = false;
      return;
    }

    const userId = this.getUserIdFromToken(token);

    const body = {
      userId: userId,
      showtimeId: 1,
      seats: this.selectedSeats.map(s => ({ row: s.row, number: s.number }))
    };

    const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });

    // Safety timeout — stop spinner after 10s no matter what
    const timeout = setTimeout(() => {
      if (this.loading) {
        this.loading = false;
        this.error = 'Request timed out. Please try again.';
      }
    }, 10000);

    this.http.post('https://localhost:7177/api/bookings', body, { headers })
      .subscribe({
        next: (res: any) => {
          clearTimeout(timeout);
          this.bookingSuccess = true;
          this.bookingRef = res.bookingRef || 'CR' + Math.random().toString(36).slice(2,8).toUpperCase();
          this.loading = false;
          this.selectedSeats.forEach(s => s.status = 'sold');
          this.selectedSeats = [];
          this.total = 0;
        },
        error: (err) => {
          clearTimeout(timeout);
          this.loading = false;
          if (err.status === 401) {
            this.error = 'Session expired. Please login again.';
          } else if (err.status === 400) {
            this.error = err.error || 'Some seats are already booked.';
          } else if (err.status === 0) {
            this.error = 'Cannot reach server. Make sure backend is running on port 7177.';
          } else {
            this.error = err.error || 'Booking failed. Please try again.';
          }
        }
      });
  }

  dismissSuccess() {
    this.bookingSuccess = false;
    this.bookingRef = '';
  }
}
