import { createRoot } from 'react-dom/client';
import './MyStylesheet.css';
import styles from './my-style.module.css';

function Header() {
    return (
        <>
            <h1 className={styles.bigred}>Hello me again!</h1>
            <p>Add a little style!</p>
        </>
    )
}

// The following is essentially a function
// const Header = () => {
//     return (
//         <>
//             <h1>Hello Style!</h1>
//             <p>Add a little style!</p>
//         </>
//     )
// };

createRoot(document.getElementById('root')).render(
    <Header />
)