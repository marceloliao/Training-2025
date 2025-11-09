import { createRoot } from 'react-dom/client';
import styled from 'styled-components';

const MyHeader = styled.h2`
padding: 10px 20 px;
background-color: #007bff;
color:white;
`;

const MyButton = styled.button`
padding: 10px 20px;
border: none;
border-radius: 4px;
background-color: ${props => props.btntype === 'primary' ? '#007bff' : '#6c757d'};
color: white;
cursor: pointer;
`;

const SecondaryButton = styled(MyButton)`
background-color: green;
`;

const SuccessButton = styled(MyButton)`
background-color: yellow;
color: black;
`;



function App() {
    return (
        <>
            <MyHeader>Welcome! I am using CSS in JS!</MyHeader>
            <MyButton btntype='primary'>Primry Button</MyButton>
            <br />
            <MyButton>Not Primry Button</MyButton>
            <br />
            <SecondaryButton>Secondary button</SecondaryButton>
            <br />
            <SuccessButton>Success button</SuccessButton>

        </>
    )
}

createRoot(document.getElementById('root')).render(
    <App />
);