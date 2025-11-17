
window.drawGrid = function (nbCols, nbRows, spriteSize) {
    const canvas = document.getElementById('myCanvas');
    const ctx = canvas.getContext('2d');
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    ctx.strokeStyle = '#cccccc'; 
    ctx.lineWidth = 1;

    for (let x = 0; x <= nbCols * spriteSize; x += spriteSize) {
        ctx.beginPath();
        ctx.moveTo(x, 0);
        ctx.lineTo(x, nbRows * spriteSize);
        ctx.stroke();
    }

    for (let y = 0; y <= nbRows * spriteSize; y += spriteSize) {
        ctx.beginPath();
        ctx.moveTo(0, y);
        ctx.lineTo(nbCols * spriteSize, y);
        ctx.stroke();
    }
};


window.drawTile = async function (tilePath, x, y, size) {
    const canvas = document.getElementById("myCanvas");
    const ctx = canvas.getContext("2d");
    
    const img = await window.loadImage(tilePath);
    ctx.drawImage(img, x * size, y * size, size, size);
};

window.imageCache = {};
window.loadImage = function (path) {
    return new Promise(resolve => {
        if (window.imageCache[path]) {
            resolve(window.imageCache[path]);
            return;
        }

        const img = new Image();
        img.src = path;
        img.onload = () => {
            window.imageCache[path] = img;
            resolve(img);
        };
    });
};


window.preloadImages = async function(interactables, player) {
    const paths = new Set();

    interactables.forEach(row => {
        row.forEach(item => paths.add(item.currentFramePath));
    });
    paths.add(player.currentFramePath);

    const promises = Array.from(paths).map(path => new Promise(resolve => {
        const img = new Image();
        img.src = path;
        img.onload = () => {
            window.imageCache[path] = img;
            resolve();
        };
    }));

    await Promise.all(promises);
};


window.drawFrame = async function (interactables, player, tileSize) {
    const canvas = document.getElementById("myCanvas");
    const ctx = canvas.getContext("2d");

    ctx.clearRect(0, 0, canvas.width, canvas.height);

    for (let i = 0; i < interactables.length; i++) {
        for (let j = 0; j < interactables[i].length; j++) {
            const item = interactables[i][j];
            await window.drawTile(item.currentFramePath, item.y, item.x, tileSize);
        }
    }

    await window.drawTile(player.currentFramePath, player.x, player.y, tileSize);
};





window.drawRaw= async function (tilePath, x, y, size) {
    const canvas = document.getElementById("myCanvas");
    const ctx = canvas.getContext("2d");

    const img = new Image();
    img.src = tilePath;
    img.onload = () => {
        ctx.drawImage(img, x, y, size, size);
    };
};
window.clearCanvas = function() {
    const canvas = document.getElementById("myCanvas");
    if (!canvas) return;
    const ctx = canvas.getContext("2d");
    ctx.clearRect(0, 0, canvas.width, canvas.height);
}
window.eraseTile = function (x, y, size) {
    const canvas = document.getElementById("myCanvas");
    const ctx = canvas.getContext("2d");

    const px = x * size;
    const py = y * size;
    ctx.clearRect(px, py, size, size);
    ctx.fillStyle = '#ffffff';

    ctx.strokeStyle = '#cccccc';
    ctx.lineWidth = 1;


    ctx.fillRect(px, py, size, size);
    ctx.strokeRect(px, py,size, size);
};

window.drawRectOnCanvas = (canvasId, x, y, width, height, color) => {
    const canvas = document.getElementById(canvasId);
    if (!canvas) return;
    const ctx = canvas.getContext('2d');
    ctx.fillStyle = color;
    ctx.fillRect(x, y, width, height);
};





document.addEventListener("contextmenu", (e) => {
    if (e.target.id === "myCanvas") e.preventDefault();
});