describe('00. Home Page - Basic Rendering and Title', () => {
  it('should load the home page and display the correct title', () => {
    cy.visit('/')
    cy.title().should('equal', 'datntdev.Microservices.Angular')
  })
})