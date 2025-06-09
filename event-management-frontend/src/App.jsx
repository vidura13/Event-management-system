
import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import EventListPage from './pages/EventListPage';
import EventDetailPage from './pages/EventDetailPage';
import EventCreatePage from './pages/EventCreatePage';
import EventUpdatePage from './pages/EventUpdatePage';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<EventListPage />} />
        <Route path="/events/new" element={<EventCreatePage />} />
        <Route path="/events/:eventId" element={<EventDetailPage />} />
        <Route path="/events/:eventId/edit" element={<EventUpdatePage />} />
      </Routes>
    </Router>
  );
}

export default App;