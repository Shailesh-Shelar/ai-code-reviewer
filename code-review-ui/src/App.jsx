import { useState } from "react";
import axios from "axios";

function App() {
  const [code, setCode] = useState("");
  const [language, setLanguage] = useState("csharp");
  const [review, setReview] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError("");
    setReview("");

    try {
      const response = await axios.post("https://localhost:7100/api/review", {
        code,
        language,
      });
      setReview(response.data.review);
    } catch (err) {
      console.error("Error:", err);
      setError("Failed to get review. Make sure the API is running.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gray-50 py-8 px-4">
      <div className="max-w-4xl mx-auto">
        <div className="bg-white rounded-lg shadow-md p-6">
          <h1 className="text-3xl font-bold text-gray-800 mb-6">
            AI Code Reviewer
          </h1>

          <form onSubmit={handleSubmit} className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Programming Language
              </label>
              <select
                value={language}
                onChange={(e) => setLanguage(e.target.value)}
                className="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              >
                <option value="csharp">C#</option>
                <option value="javascript">JavaScript</option>
                <option value="python">Python</option>
                <option value="java">Java</option>
                <option value="typescript">TypeScript</option>
              </select>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Code to Review
              </label>
              <textarea
                value={code}
                onChange={(e) => setCode(e.target.value)}
                placeholder="Paste your code here..."
                className="w-full h-64 px-4 py-3 border border-gray-300 rounded-md font-mono text-sm focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                required
              />
            </div>

            <button
              type="submit"
              disabled={loading || !code.trim()}
              className="w-full px-6 py-3 bg-blue-600 text-white font-medium rounded-md hover:bg-blue-700 disabled:bg-gray-400 disabled:cursor-not-allowed transition-colors"
            >
              {loading ? "Reviewing..." : "Review Code"}
            </button>
          </form>

          {error && (
            <div className="mt-6 p-4 bg-red-50 border border-red-200 rounded-md">
              <p className="text-red-800">{error}</p>
            </div>
          )}

          {review && (
            <div className="mt-6 p-6 bg-blue-50 border border-blue-200 rounded-md">
              <h2 className="text-lg font-semibold text-gray-800 mb-3">
                Review Result
              </h2>
              <p className="text-gray-700 whitespace-pre-wrap">{review}</p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}

export default App;
