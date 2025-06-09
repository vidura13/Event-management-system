import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import Button from "../components/Button";
import { fetchEventById, fetchAttendees, fetchEventAnalytics, registerAttendee, deleteEvent } from "../services/eventService";
import styles from "./EventDetailPage.module.scss";

export default function EventDetailPage() {
  const { eventId } = useParams();
  const navigate = useNavigate();
  const [event, setEvent] = useState(null);
  const [attendees, setAttendees] = useState([]);
  const [analytics, setAnalytics] = useState(null);
  const [registerForm, setRegisterForm] = useState({ name: "", email: "" });
  const [registerError, setRegisterError] = useState("");
  const [registerSuccess, setRegisterSuccess] = useState("");
  const [deleteError, setDeleteError] = useState("");
  const [showDeleteConfirm, setShowDeleteConfirm] = useState(false);

  useEffect(() => {
    fetchEventById(eventId).then(setEvent);
    fetchAttendees(eventId).then(setAttendees);
    fetchEventAnalytics(eventId).then(setAnalytics);
  }, [eventId]);

  async function handleRegister(e) {
    e.preventDefault();
    setRegisterError("");
    setRegisterSuccess("");
    try {
      await registerAttendee(eventId, registerForm);
      setRegisterSuccess("Registration successful!");
      setRegisterForm({ name: "", email: "" });
      setAttendees(await fetchAttendees(eventId));
    } catch (err) {
      setRegisterError(err.message);
    }
  }

  async function handleDelete() {
    setDeleteError("");
    try {
      await deleteEvent(eventId);
      navigate("/");
    } catch (err) {
      setDeleteError(err.message);
    }
  }

  if (!event) return <div className={styles.statusMsg}>Loading...</div>;

  return (
    <div className={styles.outer}>
      <div className={styles.pageTitle}>Event Management System</div>
      <div className={styles.container}>
        <div className={styles.actionRow}>
          <Button variant="secondary" onClick={() => navigate(-1)} className={styles.actionBtn}>Back to List</Button>
          <Button variant="secondary" onClick={() => navigate(`/events/${eventId}/edit`)} className={styles.actionBtn}>Edit Event</Button>
          <Button variant="danger" onClick={() => setShowDeleteConfirm(true)} className={styles.DeleteactionBtn}>Delete Event</Button>
        </div>
        {showDeleteConfirm && (
          <div className={styles.deleteConfirm}>
            <p>Are you sure you want to delete this event? This cannot be undone.</p>
            <div className={styles.deleteActions}>
              <Button variant="danger" onClick={handleDelete} className={styles.deleteBtn}>Delete</Button>
              <Button variant="secondary" onClick={() => setShowDeleteConfirm(false)} className={styles.cancelBtn}>Cancel</Button>
            </div>
            {deleteError && <div className={styles.errorMsg}>{deleteError}</div>}
          </div>
        )}
        <div className={styles.eventCard}>
          <h2 className={styles.header}>{event.name}</h2>
          <div className={styles.detailRow}><strong>Description:</strong> <span>{event.description || "—"}</span></div>
          <div className={styles.detailRow}><strong>Date:</strong> <span>{new Date(event.date).toLocaleString()}</span></div>
          <div className={styles.detailRow}><strong>Location:</strong> <span>{event.location}</span></div>
          <div className={styles.detailRow}><strong>Tags:</strong> <span>{event.tags || "—"}</span></div>
          <div className={styles.detailRow}><strong>Capacity:</strong> <span>{event.capacity}</span></div>
          <div className={styles.detailRow}><strong>Remaining Capacity:</strong> <span>{event.remainingCapacity}</span></div>
          <div className={styles.detailRow}><strong>Created By:</strong> <span>{event.createdBy}</span></div>
          {analytics && (
            <div className={styles.analytics}>
              <div><strong>Total Attendees:</strong> {analytics.totalAttendees}</div>
              <div><strong>Capacity Utilization:</strong> {(analytics.capacityUtilization * 100).toFixed(1)}%</div>
            </div>
          )}
        </div>
        <div className={styles.section}>
          <h3 className={styles.subHeader}>Attendees</h3>
          <ul className={styles.attendeeList}>
            {attendees.length === 0 ? <li className={styles.emptyMsg}>No attendees yet.</li> : attendees.map((a) => (
              <li key={a.id}>{a.name} <span className={styles.attendeeEmail}>({a.email})</span></li>
            ))}
          </ul>
        </div>
        <div className={styles.section}>
          <h3 className={styles.subHeader}>Register as Attendee</h3>
          <form className={styles.registerForm} onSubmit={handleRegister}>
            <input
              className={styles.input}
              name="name"
              placeholder="Your name"
              value={registerForm.name}
              maxLength={100}
              onChange={e => setRegisterForm(f => ({ ...f, name: e.target.value }))}
              required
            />
            <input
              className={styles.input}
              name="email"
              placeholder="Your email"
              type="email"
              value={registerForm.email}
              maxLength={150}
              onChange={e => setRegisterForm(f => ({ ...f, email: e.target.value }))}
              required
            />
            <Button type="submit" className={styles.registerBtn}>Register</Button>
          </form>
          {registerError && <div className={styles.errorMsg}>{registerError}</div>}
          {registerSuccess && <div className={styles.successMsg}>{registerSuccess}</div>}
        </div>
      </div>
    </div>
  );
}