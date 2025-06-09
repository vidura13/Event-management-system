import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import Table from "../components/Table";
import Input from "../components/Input";
import Button from "../components/Button";
import styles from './EventListPage.module.scss';
import { fetchEvents } from "../services/eventService";

const columns = [
  { key: "name", header: "Event Name" },
  { key: "date", header: "Date" },
  { key: "location", header: "Location" },
  { key: "capacity", header: "Capacity" },
  { key: "remainingCapacity", header: "Remaining Capacity" },
];

const PAGE_SIZE = 5;

export default function EventListPage() {
  const [searchBy, setSearchBy] = useState("name");
  const [searchValue, setSearchValue] = useState("");
  const [eventStatus, setEventStatus] = useState("upcoming");
  const [page, setPage] = useState(1);
  const [events, setEvents] = useState([]);
  const [totalCount, setTotalCount] = useState(0);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    setLoading(true);
    setError("");
    let filters = { page, pageSize: PAGE_SIZE };

    if (searchValue.trim()) {
      if (searchBy === "name") {
        filters.name = searchValue.trim();
      } else if (searchBy === "location") {
        filters.location = searchValue.trim();
      } else if (searchBy === "tags") {
        filters.tags = searchValue.trim();
      }
    }

    filters.datefilterType = eventStatus;

    fetchEvents(filters)
      .then((data) => {
        setEvents(data.data);
        setTotalCount(data.totalCount);
      })
      .catch((err) => setError(err.message))
      .finally(() => setLoading(false));
  }, [page, searchBy, searchValue, eventStatus]);

  const totalPages = Math.ceil(totalCount / PAGE_SIZE);

  function handleRowClick(row) {
    navigate(`/events/${row.id}`);
  }

  return (
    <div className={styles.outer}>
      <div className={styles.pageTitle}>Event Management System</div>
      <div className={styles.container}>
        <h2 className={styles.header}>All Events</h2>
        <div className={styles.filters}>
          <div className={styles.filterGroup}>
            <label className={styles.label} htmlFor="searchInput">
              {searchBy.charAt(0).toUpperCase() + searchBy.slice(1)}
            </label>
            <Input
              id="searchInput"
              label=""
              name="searchValue"
              value={searchValue}
              placeholder={`Search ${searchBy}...`}
              onChange={e => {
                setSearchValue(e.target.value);
                setPage(1);
              }}
              className={styles.input}
            />
          </div>
          <div className={styles.filterGroup}>
            <label className={styles.label} htmlFor="searchBy">Search by</label>
            <select
              className={styles.select}
              id="searchBy"
              value={searchBy}
              onChange={e => {
                setSearchBy(e.target.value);
                setPage(1);
                setSearchValue("");
              }}
            >
              <option value="name">Event Name</option>
              <option value="location">Location</option>
              <option value="tags">Tags</option>
            </select>
          </div>

          <div className={styles.filterGroup}>
            <label className={styles.label} htmlFor="eventStatus">Event Status</label>
            <select
              id="eventStatus"
              value={eventStatus}
              onChange={e => {
                setEventStatus(e.target.value);
                setPage(1);
              }}
              className={styles.select}
            >
              <option value="upcoming">Upcoming</option>
              <option value="finished">Finished</option>
            </select>
          </div>

          <Button onClick={() => navigate("/events/new")} className={styles.createBtn}>Add New Event</Button>
        </div>
        {loading && <div className={styles.statusMsg}>Loading events...</div>}
        {error && <div className={styles.errorMsg}>{error}</div>}
        <Table
          columns={columns}
          data={events.map(evt => ({
            ...evt,
            date: new Date(evt.date).toLocaleString(),
          }))}
          onRowClick={handleRowClick}
        />
        <div className={styles.pagination}>
          <Button
            variant="secondary"
            onClick={() => setPage(p => Math.max(1, p - 1))}
            disabled={page === 1}
            className={styles.paginationBtn}
          >
            Previous
          </Button>
          <span>
            Page {page} of {totalPages || 1}
          </span>
          <Button
            variant="secondary"
            onClick={() => setPage(p => Math.min(totalPages, p + 1))}
            disabled={page === totalPages || totalPages === 0}
            className={styles.paginationBtn}
          >
            Next
          </Button>
        </div>
        <div className={styles.resultsInfo}>
          Showing {events.length ? (PAGE_SIZE * (page - 1) + 1) : 0}-
          {PAGE_SIZE * (page - 1) + events.length} of {totalCount} events
        </div>
      </div>
    </div>
  );
}