import { Demo.WebPage } from './app.po';

describe('demo.web App', () => {
  let page: Demo.WebPage;

  beforeEach(() => {
    page = new Demo.WebPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
