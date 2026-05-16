import { TestBed } from "@angular/core/testing";

import { BookingServiceTs } from "./booking.service.ts";

describe("BookingServiceTs", () => {
  let service: BookingServiceTs;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BookingServiceTs);
  });

  it("should be created", () => {
    expect(service).toBeTruthy();
  });
});
