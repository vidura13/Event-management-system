import API_BASE_URL from '../config/api';


// pagination and filtering
export async function fetchEvents({ page = 1, pageSize = 10, name, location, tags, sortDate, datefilterType }) {
  let url = `${API_BASE_URL}/events?page=${page}&pageSize=${pageSize}`;
  if (name) url += `&name=${encodeURIComponent(name)}`;
  if (location) url += `&location=${encodeURIComponent(location)}`;
  if (tags) url += `&tags=${encodeURIComponent(tags)}`;
  if (sortDate) url += `&sortDate=${encodeURIComponent(sortDate)}`;
  if (datefilterType) url += `&datefilterType=${encodeURIComponent(datefilterType)}`;
  const response = await fetch(url);
  if (!response.ok) throw new Error("Failed to fetch events");
  return response.json();
}


//get event
export async function fetchEventById(eventId) {
  const response = await fetch(`${API_BASE_URL}/events/${eventId}`);
  if (!response.ok) throw new Error("Failed to fetch event");
  return response.json();
}
 

//create event
export async function createEvent(eventData) {
  const response = await fetch(`${API_BASE_URL}/events`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(eventData),
  });
  if (response.status === 400) {
    const data = await response.json();
    throw new Error(data.message || "Validation error");
  }
  if (!response.ok) throw new Error("Failed to create event");
  return response.json();
}

//update event
export async function updateEvent(eventId, eventData) {
  const response = await fetch(`${API_BASE_URL}/events/${eventId}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(eventData),
  });
  if (response.status === 400) {
    const data = await response.json();
    throw new Error(data.message || "Validation error");
  }
  if (!response.ok) throw new Error("Can't make changes to past events");
}

export async function deleteEvent(eventId) {
  const response = await fetch(`${API_BASE_URL}/events/${eventId}`, { method: "DELETE" });
  if (!response.ok) throw new Error("Failed to delete event");
}


//add attendee
export async function registerAttendee(eventId, attendeeData) {
  const response = await fetch(`${API_BASE_URL}/events/${eventId}/register`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(attendeeData),
  });
  if (response.status === 400) {
    const data = await response.json();
    if (data.message && data.message.toLowerCase().includes("past event")) {
      throw new Error("You can't register for past events");
    }
    throw new Error(data.message || "Registration failed");
  }
  if (!response.ok) throw new Error("Registration failed");
  return response.json();
}

//view attendee
export async function fetchAttendees(eventId) {
  const response = await fetch(`${API_BASE_URL}/events/${eventId}/attendees`);
  if (!response.ok) throw new Error("Failed to fetch attendees");
  return response.json();
}

//event analysis
export async function fetchEventAnalytics(eventId) {
  const response = await fetch(`${API_BASE_URL}/events/${eventId}/analytics`);
  if (!response.ok) throw new Error("Failed to fetch analytics");
  return response.json();
}