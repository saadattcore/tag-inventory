import React from "react";
import AppAlert from "./AppAlert";

export const withAlert = Component => {
  return class WithAlert extends React.Component {
    constructor(props) {
      super(props);
      this.state = { showAlert: this.props.displayAlert };
    }

    handleAlertClose = () => {
      alert("Handle Alert");
      this.setState({ showAlert: false });
    };

    componentDidUpdate(prevProps) {
      /*     alert(prevProps.displayAlert);
      alert(this.props.displayAlert); */
      if (this.props.displayAlert !== prevProps.displayAlert) {
        alert(prevProps.displayAlert);
        alert(this.props.displayAlert);
        this.setState({ showAlert: this.props.displayAlert });
      }
    }

    componentDidMount() {
      alert("Mount");
    }

    render() {
      const { type, message, title, duration, displayAlert } = this.props;
      return (
        <React.Fragment>
          <Component
            type={type}
            message={message}
            title={title}
            duration={duration}
            displayAlert={this.state.showAlert}
            onAlertClose={this.handleAlertClose.bind(this)}
          ></Component>
        </React.Fragment>
      );
    }
  };
};

export const renderAlert = (type, message, displayAlert, onAlertClose) => {
  return (
    <AppAlert
      type={type}
      message={message}
      duration={4000}
      displayAlert={displayAlert}
      onAlertClose={onAlertClose}
    ></AppAlert>
  );
};
