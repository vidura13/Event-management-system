import React from 'react';
import styles from '../styles/Button.module.css';

export default function Button({ children, onClick, type = "button", variant = "primary", ...props }) {
  return (
    <button
      className={styles[variant]}
      type={type}
      onClick={onClick}
      {...props}
    >
      {children}
    </button>
  );
}