import React, { useState, useEffect, useRef } from "react";
import {
  TextField,
  Select,
  MenuItem,
  InputLabel,
  FormControl,
  Button
} from "@material-ui/core";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import {
  getReceivedBox,
  updateReceivedBox
} from "../../../actions/receivedBoxActions";
import Actions from "../../common/reuseable/Actions";
import { Link } from "react-router-dom";
import config from "../../../config.json";
import ReceivedBoxDetail from "../received-boxes/ReceivedBoxDetail";

const ReceivedBox = props => {
  const [receivedBox, setReceivedBox] = useState({});
  const formValid = useRef(true);
  const receivedBoxID = props.match.params.id ?? -1;
  console.log(props.match.params.id);

  useEffect(() => {
    const url = `${process.env.REACT_APP_API_URL}received-box/${receivedBoxID}`;
    props.getReceivedBoxAction(url).then(response => {
      console.log(response);
      setReceivedBox({ ...response });
    });
  }, []);

  const onLinkClick = async arg => {
    console.log(arg);
    console.log(props);
    switch (arg.target.action) {
      case "back":
        props.history.goBack();
        break;

      default:
        const url = `${process.env.REACT_APP_API_URL}received-box`;
        props.updateReceivedBoxAction(url, receivedBox).then(response => {
          props.history.push(
            "/shipment/received-boxes/" + receivedBox.shipmentID
          );
        });
        break;
    }
  };

  const handleChange = ({ prop, value }) => {
    if (prop && value) {
      const cloneState = { ...receivedBox };
      cloneState[prop] = value;
      setReceivedBox(cloneState);
    }
  };

  return (
    <form>
      <div className="actions">
        <Actions
          isFormValid={formValid.current}
          onLinkClick={onLinkClick}
        ></Actions>
      </div>
      <div className="title-summary-wrapper">
        <div className="content-title">
          <h3>{receivedBoxID}</h3>
        </div>
        {receivedBoxID !== -1 && (
          <div className="shipment-detail-summary">
            <span>Summary</span> &nbsp; | &nbsp;
            <Link
              to={{
                pathname: `/received-box/tags/${receivedBoxID}`,
                receivedBox: props.receivedBox
              }}
            >
              Received Tags
            </Link>
          </div>
        )}
      </div>
      <div className="content-area">
        <div className="row">
          <div className="col-md-5">
            <ReceivedBoxDetail
              receivedBox={receivedBox}
              onChange={handleChange}
              readOnly={false}
            ></ReceivedBoxDetail>
          </div>
        </div>
      </div>
    </form>
  );
};

ReceivedBox.propTypes = {
  receivedBox: PropTypes.object.isRequired,
  updateReceivedBoxAction: PropTypes.func.isRequired,
  modifyStateAction: PropTypes.func.isRequired,
  getReceivedBoxAction: PropTypes.func.isRequired
};

const mapStateToProps = state => {
  return {
    receivedBox: state.receivedBox.receivedBox
  };
};

const mapDispatchToProps = dispatch => ({
  getReceivedBoxAction: url => dispatch(getReceivedBox(url)),
  updateReceivedBoxAction: (url, data) => dispatch(updateReceivedBox(url, data))
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(ReceivedBox);
