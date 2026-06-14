import {useState} from "react";
import { useUsername, setUsername } from "../store/user";

export default function Navbar() {
    const username = useUsername();
    const [input, setInput] = useState('');
    const [showModal, setShowModal] = useState(false);

    const handleConfirm = () => {
        if (input.trim()){
            setUsername(input.trim());
            setInput('');
            setShowModal(false);
        }
    };

    return (
        <nav>
        <div style={{ display: 'flex', alignItems: 'center', padding: '0.75rem 1.5rem' }}>
        <img src="/assets/logo.jpg" alt="logo" style={{ height: '100px' }} />
        <span style={{ marginLeft: '0.75rem' }}>Sploinky's Cinema</span>
        <button
          onClick={() => setShowModal(true)}
          style={{ marginLeft: 'auto' }}
        >
          {username ? `${username}` : 'Log in'}
        </button>
      </div>

      {showModal && (
        <div>
          <input
            autoFocus
            value={input}
            onChange={e => setInput(e.target.value)}
            onKeyDown={e => e.key === 'Enter' && handleConfirm()}
            placeholder="Enter username"
          />
          <button onClick={handleConfirm}>Confirm</button>
          <button onClick={() => setShowModal(false)}>Cancel</button>
        </div>
      )}
    </nav>
    )
};
