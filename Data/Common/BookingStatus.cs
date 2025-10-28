using System.Text.Json.Serialization;

namespace ElderConnectApi.Data.Common;

public enum BookingStatus
{
    Pending, // Waiting for payment
    Paid,  // Payment received, waiting for nurse confirmation
    Confirmed, // Nurse confirmed the booking
    InProgress, // Nurse is currently providing care
    Completed, // Care has been provided
    Cancelled, // Booking was cancelled
}