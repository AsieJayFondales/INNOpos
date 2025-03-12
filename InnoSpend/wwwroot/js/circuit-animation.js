//v7
//circuit-animation.js


document.addEventListener("DOMContentLoaded", () => {
    const canvas = document.getElementById("circuitCanvas")
    const ctx = canvas.getContext("2d")

    // Set canvas dimensions to match window size
    function resizeCanvas() {
        canvas.width = window.innerWidth
        canvas.height = window.innerHeight
        drawCircuitPattern()
    }

    window.addEventListener("resize", resizeCanvas)
    resizeCanvas()

    // Circuit node class
    class CircuitNode {
        constructor(x, y) {
            this.x = x
            this.y = y
            this.connections = []
            this.size = Math.random() * 3 + 1
            this.pulseActive = false
            this.pulseProgress = 0
            this.pulseColor = "rgba(100, 100, 255, 0.8)"
            this.pulseSpeed = Math.random() * 0.02 + 0.01
        }

        addConnection(node) {
            if (!this.connections.includes(node)) {
                this.connections.push(node)
            }
        }

        startPulse() {
            if (!this.pulseActive) {
                this.pulseActive = true
                this.pulseProgress = 0
            }
        }

        update() {
            if (this.pulseActive) {
                this.pulseProgress += this.pulseSpeed
                if (this.pulseProgress >= 1) {
                    this.pulseActive = false
                    this.pulseProgress = 0

                    // Propagate pulse to connected nodes
                    if (Math.random() > 0.3) {
                        // 70% chance to continue
                        this.connections.forEach((node) => {
                            if (Math.random() > 0.5) {
                                // 50% chance per connection
                                node.startPulse()
                            }
                        })
                    }
                }
            }
        }

        draw(ctx) {
            // Draw node
            ctx.beginPath()
            ctx.arc(this.x, this.y, this.size, 0, Math.PI * 2)
            ctx.fillStyle = this.pulseActive ? this.pulseColor : "rgba(200, 210, 255, 0.2)"
            ctx.fill()

            // Draw connections
            this.connections.forEach((node) => {
                ctx.beginPath()
                ctx.moveTo(this.x, this.y)
                ctx.lineTo(node.x, node.y)

                if (this.pulseActive) {
                    // Calculate pulse position along the line
                    const dx = node.x - this.x
                    const dy = node.y - this.y
                    const pulseX = this.x + dx * this.pulseProgress
                    const pulseY = this.y + dy * this.pulseProgress

                    // Create gradient for pulse effect
                    const gradient = ctx.createLinearGradient(this.x, this.y, node.x, node.y)
                    gradient.addColorStop(0, "rgba(180, 190, 255, 0.6)")
                    gradient.addColorStop(this.pulseProgress, this.pulseColor)
                    gradient.addColorStop(Math.min(this.pulseProgress + 0.1, 1), "rgba(180, 190, 255, 0.6)")
                    gradient.addColorStop(1, "rgba(180, 190, 255, 0.6)")

                    ctx.strokeStyle = gradient
                    ctx.lineWidth = 2
                } else {
                    ctx.strokeStyle = "rgba(180, 190, 255, 0.3)"
                    ctx.lineWidth = 1
                }

                ctx.stroke()
            })
        }
    }

    // Create circuit pattern
    let nodes = []

    function createCircuitPattern() {
        nodes = []
        const gridSize = 80
        const jitter = 20

        // Create grid of nodes with some randomness
        for (let x = 0; x < canvas.width + gridSize; x += gridSize) {
            for (let y = 0; y < canvas.height + gridSize; y += gridSize) {
                // Add some randomness to position
                const nodeX = x + (Math.random() * jitter * 2 - jitter)
                const nodeY = y + (Math.random() * jitter * 2 - jitter)

                // Skip some nodes randomly for more natural look
                if (Math.random() > 0.2) {
                    nodes.push(new CircuitNode(nodeX, nodeY))
                }
            }
        }

        // Create connections between nearby nodes
        nodes.forEach((node) => {
            nodes.forEach((otherNode) => {
                if (node !== otherNode) {
                    const dx = node.x - otherNode.x
                    const dy = node.y - otherNode.y
                    const distance = Math.sqrt(dx * dx + dy * dy)

                    // Connect nodes that are close enough
                    if (distance < gridSize * 1.5) {
                        // Only connect some nodes for a more circuit-like pattern
                        if (Math.random() > 0.6) {
                            node.addConnection(otherNode)
                        }
                    }
                }
            })
        })

        // Create some hexagonal patterns
        for (let i = 0; i < 10; i++) {
            createHexagonalPattern(Math.random() * canvas.width, Math.random() * canvas.height, Math.random() * 100 + 50)
        }

        // Start random pulses
        setInterval(() => {
            if (nodes.length > 0) {
                const randomNode = nodes[Math.floor(Math.random() * nodes.length)]
                randomNode.startPulse()
            }
        }, 1000)
    }

    function createHexagonalPattern(centerX, centerY, radius) {
        const hexNodes = []
        const sides = 6

        // Create nodes in hexagon shape
        for (let i = 0; i < sides; i++) {
            const angle = (Math.PI * 2 * i) / sides
            const x = centerX + radius * Math.cos(angle)
            const y = centerY + radius * Math.sin(angle)

            const node = new CircuitNode(x, y)
            hexNodes.push(node)
            nodes.push(node)
        }

        // Connect hexagon nodes
        for (let i = 0; i < hexNodes.length; i++) {
            hexNodes[i].addConnection(hexNodes[(i + 1) % hexNodes.length])
        }
    }

    function drawCircuitPattern() {
        ctx.clearRect(0, 0, canvas.width, canvas.height)

        // Update and draw all nodes
        nodes.forEach((node) => {
            node.update()
            node.draw(ctx)
        })

        requestAnimationFrame(drawCircuitPattern)
    }

    // Initialize
    createCircuitPattern()
    drawCircuitPattern()
})

