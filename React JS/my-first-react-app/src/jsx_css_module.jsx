import { createRoot } from 'react-dom/client';
import styles from './Button.module.css';

const Insert = () => {
    return (
        <div>
            <button className={`${styles.myButton} ${styles.primary}`}>My primary button</button>
            <br />
            <br />
            <button className={`${styles.myButton} ${styles.secondary}`}>My secondary button</button>
        </div>
    );
};

createRoot(document.getElementById('root')).render(
    <Insert />
);