document.addEventListener("DOMContentLoaded", function () {
    const messageInput = document.getElementById("messageInput");
    const sendButton = document.getElementById("sendButton");
    const chatBox = document.getElementById("chatBox");

    if (messageInput) {
        messageInput.addEventListener("input", function () {
            if (messageInput.value.length > 150) {
                messageInput.value = messageInput.value.substring(0, 150);
            }
        });
    }

    if (sendButton) {
        sendButton.addEventListener("click", function () {
            const text = messageInput.value.trim();
            if (text.length === 0) return;

            fetch("/Chat/Send", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ text: text })
            }).then(function (response) {
                if (response.ok) {
                    messageInput.value = "";
                    loadMessages();
                }
            });
        });
    }

    function loadMessages() {
        fetch("/Chat/GetMessages")
            .then(response => response.json())
            .then(data => {
                chatBox.innerHTML = "";
                data.forEach(msg => {
                    const div = document.createElement("div");
                    div.className = "message";
                    div.innerHTML =
                        "<b>" + msg.userName + "</b>: " +
                        msg.text + " <small>(" + msg.sentAt + ")</small>";
                    chatBox.appendChild(div);
                });
                chatBox.scrollTop = chatBox.scrollHeight;
            });
    }

    setInterval(loadMessages, 5000);
    loadMessages();
});