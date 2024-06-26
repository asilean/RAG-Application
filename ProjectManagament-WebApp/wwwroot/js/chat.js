﻿const chatInput = document.querySelector("#chat-input");
const sendButton = document.querySelector("#send-btn");
const chatContainer = document.querySelector(".chat-container");
const themeButton = document.querySelector("#theme-btn");
const deleteButton = document.querySelector("#delete-btn");
const module1Button = document.querySelector("#module-1");
const module2Button = document.querySelector("#module-2");
const module3Button = document.querySelector("#module-3");

let userText = null;

const loadDataFromLocalstorage = () => {
    const defaultText = `<div class="default-text">
                                    <h1>Companier</h1>
                                    <p>Start a conversation and explore the secrets of companies.<br> Your chat history will be displayed here.</p>
                                </div>`

    chatContainer.innerHTML = defaultText;
    chatContainer.scrollTo(0, chatContainer.scrollHeight); // Scroll to bottom of the chat container
}

const createChatElement = (content, className) => {
    // Create new div and apply chat, specified class and set html content of div
    const chatDiv = document.createElement("div");
    chatDiv.classList.add("chat", className);
    chatDiv.innerHTML = content;
    return chatDiv; // Return the created chat div
}

const getChatResponse = async (incomingChatDiv) => {
    const API_URL = "/Home/Conversation";
    const divElement = document.createElement("div");
    divElement.classList.add("text");
    var moduleId = document.querySelector(".module.active").getAttribute("data-module");

    // Define the properties and data for the API request
    const requestOptions = {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({
            moduleId: moduleId,
            question: userText
        })
    }

    // Send POST request to API, get response and set the reponse as paragraph element text
    try {
        const response = await (await fetch(API_URL, requestOptions)).json();
        divElement.innerHTML = response.answer.trim();
    } catch (error) { // Add error class to the paragraph element and set error text
        console.writeText(error);
        divElement.classList.add("error");
        divElement.textContent = "Oops! Something went wrong while retrieving the response. Please try again.";
    }

    // Remove the typing animation, append the paragraph element and save the chats to local storage
    incomingChatDiv.querySelector(".typing-animation").remove();
    incomingChatDiv.querySelector(".chat-details").appendChild(divElement);
    localStorage.setItem("all-chats", chatContainer.innerHTML);
    chatContainer.scrollTo(0, chatContainer.scrollHeight);
}

const copyResponse = (copyBtn) => {
    // Copy the text content of the response to the clipboard
    const reponseTextElement = copyBtn.parentElement.querySelector(".text");
    navigator.clipboard.writeText(reponseTextElement.textContent);
    copyBtn.innerHTML = '<i class="bi bi-check2"></i>';
    setTimeout(() => copyBtn.innerHTML = '<i class="bi bi-copy"></i>', 2000);
}

const showTypingAnimation = () => {
    // Display the typing animation and call the getChatResponse function
    const html = `<div class="chat-content">
                            <div class="chat-details">
                                <i class="bi bi-robot fs-3""></i>
                                <div class="typing-animation">
                                    <div class="typing-dot" style="--delay: 0.2s"></div>
                                    <div class="typing-dot" style="--delay: 0.3s"></div>
                                    <div class="typing-dot" style="--delay: 0.4s"></div>
                                </div>
                            </div>
                            <span onclick="copyResponse(this)" class="material-symbols-rounded"><i class="bi bi-copy"></i></span>
                        </div>`;
    // Create an incoming chat div with typing animation and append it to chat container
    const incomingChatDiv = createChatElement(html, "incoming");
    chatContainer.appendChild(incomingChatDiv);
    chatContainer.scrollTo(0, chatContainer.scrollHeight);
    getChatResponse(incomingChatDiv);
}

const handleOutgoingChat = () => {
    userText = chatInput.value.trim(); // Get chatInput value and remove extra spaces
    if (!userText) return; // If chatInput is empty return from here

    // Clear the input field and reset its height
    chatInput.value = "";
    chatInput.style.height = `${initialInputHeight}px`;

    const html = `<div class="chat-content">
                            <div class="chat-details">
                                <i class="bi bi-person-circle fs-3""></i>
                                <div class="text">${userText}</div>
                            </div>
                        </div>`;

    // Create an outgoing chat div with user's message and append it to chat container
    const outgoingChatDiv = createChatElement(html, "outgoing");
    chatContainer.querySelector(".default-text")?.remove();
    chatContainer.appendChild(outgoingChatDiv);
    chatContainer.scrollTo(0, chatContainer.scrollHeight);
    setTimeout(showTypingAnimation, 500);
}

deleteButton.addEventListener("click", () => {
    // Remove the chats from local storage and call loadDataFromLocalstorage function
    if (confirm("Are you sure you want to delete the chat?")) {
        var moduleId = document.querySelector(".module.active").getAttribute("data-module");
        const response = fetch(`/Home/DeleteChat?moduleId=${moduleId}`);
        console.log(response);
        loadDataFromLocalstorage();
    }
});

themeButton.addEventListener("click", () => {
    // Toggle body's class for the theme mode and save the updated theme to the local storage
    document.body.classList.toggle("light-mode");
    localStorage.setItem("themeColor", themeButton.innerText);
    themeButton.innerHTML = document.body.classList.contains("light-mode") ? '<i class="bi bi-sun-fill"></i>' : '<i class="bi bi-moon-fill"></i>';
    if (document.body.classList.contains("light-mode")) {
        module1Button.classList.remove("text-light");
        module1Button.classList.add("text-secondary-emphasis");
        module2Button.classList.remove("text-light");
        module2Button.classList.add("text-secondary-emphasis");
        module3Button.classList.remove("text-light");
        module3Button.classList.add("text-secondary-emphasis");
    } else {
        module1Button.classList.remove("text-secondary-emphasis");
        module1Button.classList.add("text-light");
        module2Button.classList.remove("text-secondary-emphasis");
        module2Button.classList.add("text-light");
        module3Button.classList.remove("text-secondary-emphasis");
        module3Button.classList.add("text-light");
    }
});

const initialInputHeight = chatInput.scrollHeight;

chatInput.addEventListener("input", () => {
    // Adjust the height of the input field dynamically based on its content
    chatInput.style.height = `${initialInputHeight}px`;
    chatInput.style.height = `${chatInput.scrollHeight}px`;
});

chatInput.addEventListener("keydown", (e) => {
    // If the Enter key is pressed without Shift and the window width is larger
    // than 800 pixels, handle the outgoing chat
    if (e.key === "Enter" && !e.shiftKey && window.innerWidth > 800) {
        e.preventDefault();
        handleOutgoingChat();
    }
});

//loadDataFromLocalstorage();
sendButton.addEventListener("click", handleOutgoingChat);