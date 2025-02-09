const express = require('express');
const axios = require('axios');

const app = express();
const PORT = process.env.PORT || 3000;

// API Key שלך
const RENDER_API_KEY = 'rnd_YLTaQHmSyLqBkVhvr6uh66N5cI58';

// Endpoint GET שמחזיר את רשימת האפליקציות
app.get('/apps', async (req, res) => {
    try {
        const response = await axios.get('https://api.render.com/v1/services', {
            headers: {
                'Authorization': `Bearer ${RENDER_API_KEY}`,
                'Content-Type': 'application/json'//
            }
        });
        res.json(response.data); // מחזיר את הנתונים ב-JSON
    } catch (error) {
        console.error('Error fetching apps:', error);
        res.status(500).json({ error: 'Failed to fetch apps' });
    }
});

// הפעלת השרת
app.listen(PORT, () => {
    console.log(`Server is running on http://localhost:${PORT}`);
});