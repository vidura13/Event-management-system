import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import Input from "../components/Input";
import Button from "../components/Button";
import { createEvent } from "../services/eventService";
import styles from "./EventCreatePage.module.scss";

export default function EventCreatePage() {
  const [eventData, setEventData] = useState({
    name: "",
    description: "",
    date: "",
    location: "",
    createdBy: "admin",
    capacity: 1,
    tags: ""
  });
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  function handleChange(e) {
    const { name, value } = e.target;
    setEventData((prev) => ({
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
        ...eventData,
        date: new Date(eventData.date).toISOString()
      };
      await createEvent(sendData);
      navigate("/");
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className={styles.outer}>
      <div className={styles.pageTitle}>Event Management System</div>
      <div className={styles.container}>
        <Button variant="secondary" onClick={() => navigate(-1)} className={styles.backBtn}>Back to List</Button>
        <div className={styles.card}>
          <h2 className={styles.header}>Add a New Event</h2>
          <form className={styles.form} onSubmit={handleSubmit}>
            <Input label="Event Name" name="name" value={eventData.name} onChange={handleChange} required className={styles.input} />
            <Input label="Date" name="date" type="datetime-local" value={eventData.date} onChange={handleChange} required className={styles.input} />
            <Input label="Location" name="location" value={eventData.location} onChange={handleChange} required className={styles.input} />
            <Input label="Capacity" name="capacity" type="number" min={1} value={eventData.capacity} onChange={handleChange} required className={styles.input} />
            <Input label="Tags (Comma-Separated)" name="tags" value={eventData.tags} onChange={handleChange} className={styles.input} />
            <Input label="Description" name="description" value={eventData.description} onChange={handleChange} className={styles.input} />
            {error && <div className={styles.errorMsg}>{error}</div>}
            <Button type="submit" disabled={loading} className={styles.submitBtn}>
              {loading ? "Creating..." : "Create Event"}
            </Button>
          </form>
        </div>
      </div>
    </div>
  );
}