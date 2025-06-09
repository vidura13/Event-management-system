import React from 'react';
import styles from '../styles/Input.module.css';

export default function Input({
  label,
  type = "text",
  name,
  value,
  onChange,
  placeholder,
  required = false,
  ...props
}) {
  return (
    <div className={styles.inputGroup}>
      {label && <label htmlFor={name}>{label}</label>}
      <input
        id={name}
        type={type}
        name={name}
        value={value}
        onChange={onChange}
        placeholder={placeholder}
        required={required}
        className={styles.input}
        {...props}
      />
    </div>
  );
}