import { MicroserviceTemplatePage } from './app.po';

describe('Microservice App', function() {
  let page: MicroserviceTemplatePage;

  beforeEach(() => {
    page = new MicroserviceTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
