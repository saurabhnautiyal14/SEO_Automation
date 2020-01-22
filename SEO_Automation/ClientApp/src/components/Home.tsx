import * as React from "react";
import { FormEvent } from "react";
import axios from "axios";

interface IResult {
  searchString: string;
  url: string;
  ranking: [];
}

interface IState {
  searchString: string;
  urlString: string;
  results: IResult[];
}

class Home extends React.PureComponent {
  state = {
    searchString: "",
    urlString: "",
    results: []
  };

  // Fix the type as syntheticEvent
  inputChangeHandler = (event: FormEvent<HTMLInputElement>) => {
    this.setState({ searchString: event.currentTarget.value });
  };

  inputURLChangeHandler = (event: FormEvent<HTMLInputElement>) => {
    this.setState({ urlString: event.currentTarget.value });
  };

  cleanSearchResult = () => {
    this.setState({ results: [] });
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
        .then(response => {
          this.setState({
            results: [...this.state.results, response.data]
          });
          console.log(response);
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
            className="btn btn-primary mr-3"
            onClick={() => {
              this.getRanking();
            }}
          >
            GET RANKING
          </button>

          <button
            type="button"
            className="btn btn-success mr-3"
            onClick={() => {
              this.cleanSearchResult();
            }}
          >
            CLEAN SEARCH RESULT
          </button>
        </form>

        <div style={{ padding: "50px" }} />
        <ul className="list-group">
          {this.state.results.map((result: IResult, index) => {
            return (
              <li key={index} className="list-group-item">
                The Ranking of {'"' + result.searchString + '"'} for URL{" "}
                {'"' + result.url + '"'} is{" "}
                {"[" + result.ranking.join(",") + "]"}
              </li>
            );
          })}
        </ul>
      </React.Fragment>
    );
  }
}

export default Home;
