import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import Input from "../components/Input";
import Button from "../components/Button";
import { fetchEventById, updateEvent } from "../services/eventService";
import styles from "./EventUpdatePage.module.scss";

export default function EventUpdatePage() {
  const { eventId } = useParams();
  const navigate = useNavigate();
  const [formData, setFormData] = useState(null);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    fetchEventById(eventId)
      .then((evt) => {
        setFormData({
          name: evt.name,
          description: evt.description || "",
          date: evt.date ? evt.date.slice(0, 16) : "",
          location: evt.location,
          tags: evt.tags || "",
          capacity: evt.capacity
        });
      })
      .catch(() => setError("Event not found."));
  }, [eventId]);

  function handleChange(e) {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: name === "capacity" ? Number(value) : value
    }));
  }

  async function handleSubmit(e) {
    e.preventDefault();
    setError("");
    setLoading(true);
    try {
      const sendData = {
        ...formData,
        date: new Date(formData.date).toISOString()
      };
      await updateEvent(eventId, sendData);
      navigate(`/events/${eventId}`);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }

  if (!formData) return <div className={styles.statusMsg}>Loading...</div>;

  return (
    <div className={styles.outer}>
      <div className={styles.pageTitle}>Event Management System</div><div className={styles.container}>
        <Button variant="secondary" onClick={() => navigate(-1)} className={styles.backBtn}>Back</Button>
        <div className={styles.card}>
          <h2 className={styles.header}>Update Event</h2>
          <form className={styles.form} onSubmit={handleSubmit}>
            <Input label="Event Name" name="name" value={formData.name} onChange={handleChange} required className={styles.input} />
            <Input label="Date" name="date" type="datetime-local" value={formData.date} onChange={handleChange} required className={styles.input} />
            <Input label="Location" name="location" value={formData.location} onChange={handleChange} required className={styles.input} />
            <Input label="Capacity" name="capacity" type="number" min={1} value={formData.capacity} onChange={handleChange} required className={styles.input} />
            <Input label="Tags (Comma-Separated)" name="tags" value={formData.tags} onChange={handleChange} className={styles.input} />
            <Input label="Description" name="description" value={formData.description} onChange={handleChange} className={styles.input} />
            {error && <div className={styles.errorMsg}>{error}</div>}
            <Button type="submit" disabled={loading} className={styles.submitBtn}>
              {loading ? "Saving..." : "Save Changes"}
            </Button>
          </form>
        </div>
      </div>
    </div>
  );
}