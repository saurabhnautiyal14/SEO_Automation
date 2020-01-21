import * as React from "react";
import { FormEvent } from "react";
//import axios, { AxiosResponse } from "axios";
import axios from "axios";

//const responseBody = (response: AxiosResponse) => response.data;

class Home extends React.PureComponent {
  // Need to convert to typeScript
  state = {
    searchString: "",
    urlString: ""
  };

  // Fix the type as syntheticEvent
  inputChangeHandler = (event: FormEvent<HTMLInputElement>) => {
    this.setState({ searchString: event.currentTarget.value });
  };

  inputURLChangeHandler = (event: FormEvent<HTMLInputElement>) => {
    this.setState({ urlString: event.currentTarget.value });
  };

  // TODO make this async
  getRanking = () => {
    try {
      axios
        .get("https://localhost:5001/api/searchRating", {
          params: {
            searchString: this.state.searchString,
            url: this.state.urlString
          }
        })
        .then(responseBody => {
          console.log(responseBody);
        });
    } catch (error) {
      console.error(error);
    }
  };

  render() {
    return (
      <React.Fragment>
        <form>
          <div className="form-group">
            <label>SearchString</label>
            <input
              type="text"
              name="searchString"
              className="form-control"
              value={this.state.searchString}
              onChange={event => this.inputChangeHandler(event)}
            />
          </div>
          <div className="form-group">
            <label>URL</label>
            <input
              type="text"
              name="urlString"
              className="form-control"
              value={this.state.urlString}
              onChange={event => this.inputURLChangeHandler(event)}
            />
          </div>
          <button
            type="button"
            className="btn btn-primary btn-lg"
            onClick={() => {
              this.getRanking();
            }}
          >
            GET RANKING
          </button>
        </form>
      </React.Fragment>
    );
  }
}

export default Home;
