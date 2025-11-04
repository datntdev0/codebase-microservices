describe('00. Authentication', () => {
  before(() => { 
    cy.clearAllCookies();
    cy.clearAllLocalStorage();
    cy.clearAllSessionStorage();
  }) 

  it('should redirect to sso page if not authenticated', () => {
    cy.visit(Cypress.env('baseUrl'));
    cy.url().should('include', `${Cypress.env('ssoUrl')}/auth/signin`);
  })

  it('should redirect to dashboard when sign in succeeded', () => {
    cy.visit(Cypress.env('baseUrl'));

    cy.origin(Cypress.env('ssoUrl'), () => {
      cy.get('input[name="Model.Email"]').type('admin@datntdev.com');
      cy.get('input[name="Model.Password"]').type('Admin@123');
      cy.get('button[type="submit"]').click();
    });

    // Should redirect back to the main app after successful login
    cy.url().should('include', Cypress.env('baseUrl'));
    cy.url().should('not.include', Cypress.env('ssoUrl'));
    cy.get('h4').should('contain', 'Dashboard');
  });

  it('should redirect to sso page when sign out succeeded', () => {
    cy.visit(Cypress.env('baseUrl'));
    cy.get('h4').should('contain', 'Dashboard');
    cy.title().should('eq', 'datntdev.Microservices - Dashboard');
    
    cy.get('.app-navbar .cursor-pointer').click();
    cy.get('#sign-out-button').click();
    cy.url().should('include', `${Cypress.env('ssoUrl')}/auth/signin`);
  })
  
  it('should show alert popup when sign in failed', () => {
    cy.visit(Cypress.env('baseUrl'));

    cy.origin(Cypress.env('ssoUrl'), () => {
      cy.get('input[name="Model.Email"]').type('admin@datntdev.com');
      cy.get('input[name="Model.Password"]').type('Admin@123456');
      cy.get('button[type="submit"]').click();
      cy.get('.swal2-title').should('be.visible');
      cy.get('.swal2-title').should('contain', 'Login Failed');
    });
  });
})